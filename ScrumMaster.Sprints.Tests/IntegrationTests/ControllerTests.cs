using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using ScrumMaster.Sprints.Infrastructure.Commands;
using ScrumMaster.Sprints.Infrastructure.DataAccess;
using ScrumMaster.Sprints.Infrastructure.Tests.Commons;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ScrumMaster.Sprints.Infrastructure.Tests.IntegrationTests
{
    public class ControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IServiceScope _scope;
        private readonly SprintDbContext _dbContext;
        public ControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _scope = factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<SprintDbContext>();
        }
        [Fact]
        public async Task GetSprintById_WhenCanNotFindSprint_Should_ReturnNull()
        {
            //Arrange
            ClearDb();
            var token = CreateToken();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            var response = await client.GetAsync("/Sprint/GetSprintById?sprintId=00000000-0000-0000-0000-000000000001");
            //Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
        [Fact]
        public async Task CreateSprint_WhenCommandIsNull_Should_ReturnBadRequestWithMessage()
        {
            //Arrange
            ClearDb();
            var token = CreateToken();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            var response = await client.PostAsync("/Sprint/CreateSprint", new StringContent(JsonSerializer.Serialize(""), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            //Assert
            Assert.Contains("Command_Cannot_Be_Null", result);
        }
        [Fact]
        public async Task CreateSprint_WhenCommandIsCorrect_Should_ReturnGuid()
        {
            //Arrange
            ClearDb();
            var token = CreateToken();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var command = new CreateSprintCommand() { Name = "TestSprint", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10) };
            //Act
            var response = await client.PostAsync("/Sprint/CreateSprint", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            try
            {
                var newSprintId = JsonSerializer.Deserialize<Guid>(result);
                Assert.True(newSprintId != Guid.Empty);
            }
            //Assert
            catch(Exception ex)
            {
                Assert.True(false);
            }
        }
        [Fact]
        public async Task UpdateSprint_WhenCommandIsNull_Should_ReturnBadRequestWithMessage()
        {
            //Arrange
            ClearDb();
            var token = CreateToken();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            var response = await client.PutAsync("/Sprint/UpdateSprint", new StringContent(JsonSerializer.Serialize(""), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            //Assert
            Assert.Contains("Command_Cannot_Be_Null", result);
        }

        [Fact]
        public async Task UpdateSprint_WhenUpdatedSprintDoesnNotExist_Should_BadRequestWithMessage()
        {
            //Arrange
            ClearDb();
            var token = CreateToken();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var command = new UpdateSprintCommand() { SprintId = Guid.Empty };
            //Act
            var response = await client.PutAsync("/Sprint/UpdateSprint", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            //Assert
            Assert.Contains("Cannot_Find_Sprint_In_Database", result);
        }
        private void ClearDb()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
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
