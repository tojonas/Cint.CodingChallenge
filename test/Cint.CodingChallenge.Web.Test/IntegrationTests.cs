using Cint.CodingChallenge.Data;
using Cint.CodingChallenge.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Cint.CodingChallenge.Web.Test
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _client = factory.CreateClient();
        }

        async Task CreateDatabase()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
                await db.InitializeAsync();
            }
        }

        [Fact]
        public async Task SurveySearch_WithUniqueFilter_ShouldReturn_MatchingItem()
        {
            var response = await _client.GetFromJsonAsync<SurveyViewModel[]>("/survey/search?name=Fruit Survey");

            // Integration test assertions
            response.ShouldNotBeNull();
            response.Length.ShouldBe(3);
        }

        [Fact]
        public async Task SurveySearch_WithoutFilter_ShouldReturn_AllItems()
        {
            var response = await _client.GetFromJsonAsync<SurveyViewModel[]>("/survey/search");

            // Integration test assertions
            response.ShouldNotBeNull();
            response.Length.ShouldBe(12); // TODO: Now we test, need to clean db between tests
        }

        [Fact]
        public async Task SurveyCreate_ShouldCreateAndReturn_NewSurvey()
        {
            var newSurvey = new SurveyCreateViewModel
            {
                Name = "Test one name",
                Description = "Test one description",
                IncentiveEuros = 1.2,
                LengthMinutes = 5
            };
            var response = await _client.PostAsJsonAsync<SurveyCreateViewModel>("/survey", newSurvey);

            // Integration test assertions
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<SurveyViewModel>();
            created.ShouldNotBeNull();
            created.Id.ShouldNotBe(Guid.Empty);
            created.Name.ShouldBe(newSurvey.Name);
            created.Description.ShouldBe(newSurvey.Description);
            created.IncentiveEuros.ShouldBe(newSurvey.IncentiveEuros);
            created.LengthMinutes.ShouldBe(newSurvey.LengthMinutes);

            response.Headers.GetValues("Location").First().ShouldBe($"survey/{created.Id}");
        }

        [Fact]
        public async Task SurveyCreateWithEmptyName_ShouldReturn_BadRequest()
        {
            var newSurvey = new SurveyCreateViewModel
            {
                Name = "",
                Description = "Test one description",
                IncentiveEuros = 1.2,
                LengthMinutes = 5
            };
            var response = await _client.PostAsJsonAsync<SurveyCreateViewModel>("/survey", newSurvey);

            // Integration test assertions
            response.ShouldNotBeNull();
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }


        public async Task InitializeAsync()
        {
            await CreateDatabase();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
