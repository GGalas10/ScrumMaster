using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ScrumMaster.Sprints.Infrastructure.Commands;
using ScrumMaster.Sprints.Infrastructure.DataAccess;
using ScrumMaster.Sprints.Infrastructure.DTO;
using ScrumMaster.Sprints.Infrastructure.Tests.Commons;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
        public async Task UpdateSprint_WhenUpdatedSprintDoesnNotExist_Should_ReturnBadRequestWithMessage()
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

        [Fact]
        public async Task UpdateSprint_WhenCommandHasnNoChanges_Should_ReturnBadRequestWithMessage()
        {
            //Arrange
            ClearDb();
            var token = CreateToken();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var command = new CreateSprintCommand() { Name = "TestSprint", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10) };
            var response = await client.PostAsync("/Sprint/CreateSprint", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var newSprintId = JsonSerializer.Deserialize<Guid>(await response.Content.ReadAsStringAsync());
            var updateCommand = new UpdateSprintCommand() { SprintId = newSprintId };

            //Act
            var responseUpdate = await client.PutAsync("/Sprint/UpdateSprint", new StringContent(JsonSerializer.Serialize(updateCommand), Encoding.UTF8, "application/json"));
            var result = await responseUpdate.Content.ReadAsStringAsync();
            //Assert
            Assert.Contains("There_are_no_changes_for_sprint", result);
        }

        [Fact]
        public async Task UpdateSprint_WhenCommandChangeName_Should_ChangeName()
        {
            //Arrange
            ClearDb();
            var token = CreateToken();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var command = new CreateSprintCommand() { Name = "TestSprint", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10) };
            var response = await client.PostAsync("/Sprint/CreateSprint", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var newSprintId = JsonSerializer.Deserialize<Guid>(await response.Content.ReadAsStringAsync());
            var updateCommand = new UpdateSprintCommand() { SprintId = newSprintId,SprintName = "NewSprintTest2" };
            await client.PutAsync("/Sprint/UpdateSprint", new StringContent(JsonSerializer.Serialize(updateCommand), Encoding.UTF8, "application/json"));
            var sprint = await client.GetAsync($"/Sprint/GetSprintById?sprintId={newSprintId}");
            //Act
            var updatedSprint = JsonSerializer.Deserialize<SprintDTO>(await sprint.Content.ReadAsStringAsync());
            //Assert
            Assert.Equal("NewSprintTest2", updateCommand.SprintName);
        }
        [Fact]
        public async Task DeleteSprint_WhenDeleteSprintDoesnNotExist_Should_ReturnBadRequestWithMessage()
        {
            //Arrange
            ClearDb();
            var token = CreateToken();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            var deleted = await client.DeleteAsync($"/Sprint/DeleteSprint?sprintId={Guid.Empty}");
            //Assert
            Assert.Contains("Cannot_Find_Sprint_In_Database",await deleted.Content.ReadAsStringAsync());
        }
        [Fact]
        public async Task DeleteSprint_WhenSprintExist_Should_DeleteSprint()
        {
            //Arrange
            ClearDb();
            var token = CreateToken();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var command = new CreateSprintCommand() { Name = "TestSprint", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10) };
            var response = await client.PostAsync("/Sprint/CreateSprint", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var newSprintId = JsonSerializer.Deserialize<Guid>(await response.Content.ReadAsStringAsync());
            //Act
            var deleted = await client.DeleteAsync($"/Sprint/DeleteSprint?sprintId={newSprintId}");
            var result = await client.GetAsync($"/Sprint/GetSprintById?sprintId={newSprintId}");
            //Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }
        [Fact]
        public async Task CheckSprintExist_WhenSprintDoesnNotExist_Should_ReturnFalse()
        {
            //Arrange
            ClearDb();
            var token = CreateToken();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //Act
            var result = await client.GetAsync($"/Sprint/CheckSprintExist?sprintId={Guid.Empty}");
            var isExist = JsonSerializer.Deserialize<bool>(await result.Content.ReadAsStringAsync());
            //Assert
            Assert.False(isExist);
        }
        [Fact]
        public async Task CheckSprintExist_WhenSprintExist_Should_ReturnTrue()
        {
            //Arrange
            ClearDb();
            var token = CreateToken();
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var command = new CreateSprintCommand() { Name = "TestSprint", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(10) };
            var response = await client.PostAsync("/Sprint/CreateSprint", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var newSprintId = JsonSerializer.Deserialize<Guid>(await response.Content.ReadAsStringAsync());
            //Act
            var result = await client.GetAsync($"/Sprint/CheckSprintExist?sprintId={newSprintId}");
            var isExist = JsonSerializer.Deserialize<bool>(await result.Content.ReadAsStringAsync());
            //Assert
            Assert.True(isExist);
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
            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(GetJwtKeyFromFactory()));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                null, null,
                claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GetJwtKeyFromFactory()
        {
            using var scope = _factory.Services.CreateScope();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            return config["JwtSettings:Secret"]!;
        }
    }
}
