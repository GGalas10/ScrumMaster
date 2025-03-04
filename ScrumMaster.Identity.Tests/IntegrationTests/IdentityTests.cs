using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ScrumMaster.Identity.Infrastructure.Commands;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Xunit;

namespace ScrumMaster.Identity.Tests.IntegrationTests
{
    public class IdentityTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public IdentityTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async void AuthController_WhenSendNull_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(""), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();


            //Assert
            Assert.Contains("Command_Is_Null", content);
        }
        [Fact]
        public async void AuthController_WhenSendCommandWithoutEmail_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", password = "PasswordTest" };

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("Email_Cannot_Be_Null", content);
        }
        [Fact]
        public async void AuthController_WhenSendCommandWithoutPassword_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail" };

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("Password_Cannot_Be_Null", content);
        }
        [Fact]
        public async void AuthController_WhenPasswordHasNotAlphanumeric_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail",password = "TestPassword" };

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("PasswordRequiresNonAlphanumeric", content);
        }
        [Fact]
        public async void AuthController_WhenPasswordIsShort_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail",password = "Test$123" };

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("PasswordTooShort", content);
        }
        [Fact]
        public async void AuthController_WhenPasswordHasNotUppercase_Should_ReturnBadRequestWithErrorMessage()
        {
            //Arrange
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail",password = "test$123" };

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Contains("PasswordRequiresUpper", content);
        }
        [Fact]
        public async void AuthController_WhenCommandIsCorrect_Should_ReturnOk()
        {
            //Arrange
            var client = _factory.CreateClient();
            var command = new RegisterUserCommand() { firstName = "FirstTest", lastName = "LastName", email = "TestEmail",password = "TestTest$123" };

            //Act
            var response = await client.PostAsync("Register", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Equal(HttpStatusCode.OK,response.StatusCode);
        }
    }
}
