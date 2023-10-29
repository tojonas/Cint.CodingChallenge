using Cint.CodingChallenge.Core.Interfaces;
using Cint.CodingChallenge.Data;
using Cint.CodingChallenge.Data.Repositories;
using Cint.CodingChallenge.Web.Extensions;
using Cint.CodingChallenge.Web.Filters;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services
    .AddEndpointsApiExplorer()
    .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Database"))
    .AddTransient<DatabaseInitializer>()
    .AddScoped(typeof(IRepository<,>), typeof(Repository<,>))
    .AddSwaggerGen(options =>
    {
        options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    })
    .Configure<RequestLocalizationOptions>(options =>
    {
        //https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization/select-language-culture?view=aspnetcore-7.0
        //https://learn.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-6.0
        options.DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture);
    })
    .AddControllersWithViews(options =>
    {
        options.Filters.Add<ModelStateValidationFilter>();
    })
    .AddControllersAsServices();

var app = builder.Build();
// Create the database
using (var scope = app.Services.CreateScope())
{
    var dbInit = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await dbInit.InitializeAsync();
}


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.ConfigureExceptionHandler()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthorization()
    .UseRequestLocalization();
app.MapDefaultControllerRoute();
app.Run();

public partial class Program { }
