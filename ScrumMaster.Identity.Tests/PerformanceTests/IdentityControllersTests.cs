using Microsoft.Extensions.DependencyInjection;
using NBomber.Contracts.Stats;
using NBomber.CSharp;
using ScrumMaster.Identity.Infrastructure.Commands;
using ScrumMaster.Identity.Infrastructure.DataAccesses;
using ScrumMaster.Identity.Tests.Common;
using System.Text;
using System.Text.Json;

namespace ScrumMaster.Identity.Tests.PerformanceTests
{
    public class IdentityControllersTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IServiceScope _scope;
        private readonly UserDbContext _dbContext;
        public IdentityControllersTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _scope = factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<UserDbContext>();
        }
        [Fact]
        public async void NBomberTest()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
            var client = _factory.CreateClient();
            var httpClient = new HttpClient();
            var registerCommand = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail", password = "TestTest$123" };

            var scenario = Scenario.Create("http_scenario", async context =>
            {
                var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(""), Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();
                return content.Contains("Command_Is_Null") ? Response.Ok() : Response.Fail();
            })
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                Simulation.Inject(rate: 300, interval: TimeSpan.FromSeconds(1), during: TimeSpan.FromSeconds(20)));
                

            NBomberRunner
                .RegisterScenarios(scenario)
                .WithReportFolder("NBomber")
                .WithReportFormats(ReportFormat.Html)
                .Run();
        }
    }
}
