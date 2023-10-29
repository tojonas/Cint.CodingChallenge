using Cint.CodingChallenge.Core.Interfaces;
using Cint.CodingChallenge.Data;
using Cint.CodingChallenge.Data.Repositories;
using Cint.CodingChallenge.Model;
using Cint.CodingChallenge.Web.Controllers;
using Cint.CodingChallenge.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using Xunit;

namespace Cint.CodingChallenge.Web.Test.Controllers
{
    //https://github.com/JonPSmith/EfCore.TestSupport
    public class SurveyControllerTests
    {
        private readonly Survey[] _surveys = new[]
        {
            new Survey
            {
                Id = Guid.NewGuid(),
                Name = "Test one name",
                Description = "Test one description",
                IncentiveEuros = 1.2,
                LengthMinutes = 5
            },
            new Survey
            {
                Id = Guid.NewGuid(),
                Name = "Test two name",
                Description = "Test two description",
                IncentiveEuros = 1.1,
                LengthMinutes = 4
            },
            new Survey
            {
                Id = Guid.NewGuid(),
                Name = "Test three name",
                Description = "Test three description",
                IncentiveEuros = 2.1,
                LengthMinutes = 4
            }
        };

        public SurveyControllerTests()
        {
        }

        async Task WithInMemoryDatabase(Func<IRepository<Survey, Guid>, Task> test)
        {
            var options = SqliteInMemory.CreateOptions<ApplicationDbContext>();
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                // Seed
                await context.Surveys.AddRangeAsync(_surveys);
                await context.SaveChangesAsync();

                var repository = new Repository<Survey, Guid>(context);
                await test(repository);
            }
        }

        async Task WithSurveyController(Func<SurveyController, Task> test)
        {
            var logger = new Mock<ILogger<SurveyController>>();
            await WithInMemoryDatabase(r => test(new SurveyController(r, logger.Object)));
        }

        [Theory]
        [InlineData("", 3, new[] { "Test one name", "Test two name", "Test three name" })]
        [InlineData(null, 3, new[] { "Test one name", "Test two name", "Test three name" })]
        public async Task SearchSurveysWithEmptyFilterReturnsAllSurveys(string? search, int resultCount, string[] matches)
        {
            await WithSurveyController(async controller =>
            {
                var response = await controller.Search(search);

                response.ShouldNotBeNull();

                var objectResponse = response.ShouldBeOfType<OkObjectResult>();
                objectResponse.StatusCode.ShouldBe(200);

                objectResponse.Value.ShouldNotBeNull();
                objectResponse.Value.ShouldBeOfType<SurveyViewModel[]>();
                // assert surveys are correct
                SurveyViewModel[] surveys = (SurveyViewModel[])objectResponse.Value;
                surveys.Length.ShouldBe(resultCount);
                foreach (var name in matches)
                {
                    surveys.ShouldContain(s => s.Name == name);
                }
            });
        }

        [Theory]
        [InlineData("two", 1, "Test two name")]
        [InlineData("TWO", 1, "Test two name")]
        [InlineData("TwO", 1, "Test two name")]
        public async Task SearchFiltersSurveysByCaseInsensitiveSubstring(string search, int resultCount, string fullMatch)
        {
            await WithSurveyController(async controller =>
            {
                var response = await controller.Search(search);

                response.ShouldNotBeNull();

                var objectResponse = response.ShouldBeOfType<OkObjectResult>();
                objectResponse.StatusCode.ShouldBe(200);

                objectResponse.Value.ShouldNotBeNull();
                objectResponse.Value.ShouldBeOfType<SurveyViewModel[]>();

                SurveyViewModel[] surveys = (SurveyViewModel[])objectResponse.Value;
                // assert surveys are correct
                surveys.Length.ShouldBe(resultCount);
                surveys[0].Name.ShouldBe(fullMatch);
            });
        }

        [Fact]
        public async Task SearchSortsSurveysByEfficiency()
        {
            // efficiency can be calulated as follows:
            // efficiency = incentive (euros) / length (minutes)
            await WithSurveyController(async controller =>
            {
                var response = await controller.Search(string.Empty);

                response.ShouldNotBeNull();

                var objectResponse = response.ShouldBeOfType<OkObjectResult>();
                objectResponse.StatusCode.ShouldBe((int)HttpStatusCode.OK);

                objectResponse.Value.ShouldNotBeNull();
                objectResponse.Value.ShouldBeOfType<SurveyViewModel[]>();

                SurveyViewModel[] surveys = (SurveyViewModel[])objectResponse.Value;
                // assert surveys are correct
                surveys.Length.ShouldBe(_surveys.Length);
                surveys[0].Name.ShouldBe(_surveys[2].Name);
                surveys[1].Name.ShouldBe(_surveys[1].Name);
                surveys[2].Name.ShouldBe(_surveys[0].Name);
            });
        }

        [Fact]
        public async Task CanAddNewSurvey()
        {
            await WithSurveyController(async controller =>
            {
                var survey = new SurveyCreateViewModel
                {
                    Name = "Test four name",
                    Description = "Test four description",
                    IncentiveEuros = 2.2,
                    LengthMinutes = 6
                };

                var response = await controller.Create(survey);

                response.ShouldNotBeNull();

                var objectResponse = response.ShouldBeOfType<CreatedResult>();
                objectResponse.StatusCode.ShouldBe((int)HttpStatusCode.Created);

                objectResponse.Value.ShouldNotBeNull();
                objectResponse.Value.ShouldBeOfType<SurveyViewModel>();

                var addedSurvey = (SurveyViewModel)objectResponse.Value;
                objectResponse.Location.ShouldBe($"survey/{addedSurvey.Id}");
            });
        }
    }
}
