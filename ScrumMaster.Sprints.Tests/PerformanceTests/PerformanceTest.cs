using Microsoft.Extensions.DependencyInjection;
using NBomber.CSharp;
using NBomber.Contracts.Stats;
using ScrumMaster.Sprints.Infrastructure.DataAccess;
using ScrumMaster.Sprints.Infrastructure.Tests.Commons;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http.Headers;

namespace ScrumMaster.Sprints.Tests.PerformanceTests
{
    public class PerformanceTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IServiceScope _scope;
        private readonly SprintDbContext _dbContext;
        public PerformanceTest(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _scope = factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<SprintDbContext>();
        }
        [Fact]
        public async void NBomberTest()
        {
            var token = CreateToken();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var scenario = Scenario.Create("http_scenario", async context =>
            {
                var response = await client.GetAsync("/Sprint/GetSprintById?sprintId=00000000-0000-0000-0000-00000000000");
                return response.StatusCode == System.Net.HttpStatusCode.NoContent ? Response.Ok() : Response.Fail();
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
        private string CreateToken()
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "00000000-0000-0000-0000-000000000001"),
            new Claim(JwtRegisteredClaimNames.Email, "Email@test.pl"),
            new Claim(JwtRegisteredClaimNames.Name, $"Test Name"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("s+Qr8+VhSWEHHuwyqwP0kNvtg3HCSEX25A3MP1iENH4="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                null, null,
                claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
