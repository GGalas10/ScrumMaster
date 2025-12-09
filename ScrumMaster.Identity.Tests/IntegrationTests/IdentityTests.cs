using Microsoft.Extensions.DependencyInjection;
using ScrumMaster.Identity.Infrastructure.Commands;
using ScrumMaster.Identity.Infrastructure.DataAccesses;
using ScrumMaster.Identity.Tests.Common;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ScrumMaster.Identity.Tests.IntegrationTests
{
    public class IdentityTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly IServiceScope _scope;
        private readonly UserDbContext _dbContext;
        public IdentityTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _scope = factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<UserDbContext>();
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }
        [Fact]
        public async void Register_WhenSendNull_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(""), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();


            //Assert
            Assert.Contains("Command_Is_Null", content);
        }
        [Fact]
        public async void Register_WhenSendCommandWithoutEmail_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", password = "PasswordTest" };

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("Email_Cannot_Be_Null", content);
        }
        [Fact]
        public async void Register_WhenSendCommandWithoutPassword_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail" };

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("Password_Cannot_Be_Null", content);
        }
        [Fact]
        public async void Register_WhenPasswordHasNotAlphanumeric_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail",password = "TestPassword", confirmPassword = "TestPassword", userName = "TestUserName" };

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("PasswordRequiresNonAlphanumeric", content);
        }
        [Fact]
        public async void Register_WhenPasswordIsShort_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail",password = "Test$123", confirmPassword = "Test$123", userName = "TestUserName" };

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("Password_Is_Too_Short", content);
        }
        [Fact]
        public async void Register_WhenPasswordHasNotUppercase_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail",password = "testtest$123", confirmPassword = "testtest$123", userName = "TestUserName" };

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("PasswordRequiresUpper", content);
        }
        [Fact]
        public async void Register_WhenEmailIsAlreadyExist_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail",password = "TestTest$123", confirmPassword = "TestTest$123", userName = "TestUserName" };

            //Act
            await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("User_Email_Already_Exist", content);
        }
        [Fact]
        public async void Register_WhenUserNameIsAlreadyExist_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail",password = "TestTest$123", confirmPassword = "TestTest$123", userName = "TestUserName" };

            //Act
            await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            command.email = "EmailTest";
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("User_Name_Already_Exist", content);
        }
        [Fact]
        public async void Register_WhenCommandIsCorrect_Should_ReturnOk()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail",password = "TestTest$123", confirmPassword = "TestTest$123", userName = "TestUserName" };

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }
        [Fact]
        public async void Login_WhenCommandIsNull_Should_ReturnBadRequestWithError()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail", password = "TestTest$123", confirmPassword = "TestTest$123", userName = "TestUserName" };

            //Act
            await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            command.email = "EmailTest";
            var response = await client.PostAsync("Login", new StringContent(JsonSerializer.Serialize(""), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("Command_Is_Null", content);
        }
        [Fact]
        public async void Login_WhenUserDoesNotExist_Should_ReturnBadRequestWithError()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();
            var registerCommand = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail", password = "TestTest$123", confirmPassword = "TestTest$123", userName = "TestUserName" };
            var command = new LoginUserCommand() { email = "TestEmail", password = "TestTest$123" };

            //Act
            await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(registerCommand), Encoding.UTF8, "application/json"));
            command.email = "EmailTest";
            var response = await client.PostAsync("Login", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("Wrong_Credentials", content);
        }
        [Fact]
        public async void Login_WhenPasswordIsInCorrect_Should_ReturnBadRequestWithError()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();
            var registerCommand = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail", password = "TestTest$123", confirmPassword = "TestTest$123", userName = "TestUserName" };
            var command = new LoginUserCommand() { email = "TestEmail", password = "TestTest123" };

            //Act
            await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(registerCommand), Encoding.UTF8, "application/json"));
            var response = await client.PostAsync("Login", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("Wrong_Credentials", content);
        }
        [Fact]
        public async void Login_WhenCommandIsCorrect_Should_ReturnOkWithJwt()
        {
            //Arrange
            ClearDatabase();
            var client = _factory.CreateClient();
            var registerCommand = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail", password = "TestTest$123", confirmPassword = "TestTest$123", userName = "TestUserName" };
            var command = new LoginUserCommand() { email = "TestEmail", password = "TestTest$123" };

            //Act
            await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(registerCommand), Encoding.UTF8, "application/json"));
            var response = await client.PostAsync("Login", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        private void ClearDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }
    }
}
