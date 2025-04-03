using Microsoft.Extensions.Options;
using Moq;
using ScrumMaster.Identity.Core.Models;
using ScrumMaster.Identity.Infrastructure.Commands;
using ScrumMaster.Identity.Infrastructure.Contracts;
using ScrumMaster.Identity.Infrastructure.DTO;
using ScrumMaster.Identity.Infrastructure.Implementations;
using ScrumMaster.Identity.Tests.Common;

namespace ScrumMaster.Identity.Tests.ServicesTests
{
    public class UserServiceTests
    {
        IOptions<JwtSettings> generalOptions = Options.Create<JwtSettings>(new JwtSettings() { secret="sadafsdsf676sdf6fs7df0h1j2#$%TDS*_(SDF",ExpirationMinutes=1});
        [Fact]
        public async void RegisterUser_WhenCommandNull_Should_ThrowException()
        {
            //Arrange
            var userList = MockUserManager.GetUsersList();
            IJwtHandler jwtHandler = new JwtHandler(generalOptions);
            var db = InitialDatabase.InitDatabase();
            IRefreshTokenService refreshToken = new RefreshTokenService(db,jwtHandler);
            var manager = MockUserManager.GetUserManager(userList);
            var service = new UserService(manager.Object,jwtHandler, refreshToken);
            //Act
            try
            {
                await service.RegisterUser(null);
            }

            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Command_Is_Null", ex.Message);
            }
        }
        [Fact]
        public async void RegisterUser_WhenUserExistWithCommandEmail_Should_ThrowException()
        {
            //Arrange
            var userList = MockUserManager.GetUsersList();
            userList.Add(new AppUser() { UserName = "Test", Email = "TestEmail" });
            IJwtHandler jwtHandler = new JwtHandler(generalOptions);
            var manager = MockUserManager.GetUserManager(userList);
            var db = InitialDatabase.InitDatabase();
            IRefreshTokenService refreshToken = new RefreshTokenService(db, jwtHandler);
            var service = new UserService(manager.Object, jwtHandler,refreshToken);
            var command = new RegisterUserCommand()
            {
                email = "TestEmail",
                password = "Test"
            };
            //Act
            try
            {
                await service.RegisterUser(command);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("User_Email_Already_Exist", ex.Message);
            }
        }
        [Fact]
        public async void RegisterUser_WhenUserExistWithCommandUserName_Should_ThrowException()
        {
            //Arrange
            var userList = MockUserManager.GetUsersList();
            userList.Add(new AppUser() { UserName = "Test Name", Email = "TestEmail" });
            IJwtHandler jwtHandler = new JwtHandler(generalOptions);
            var manager = MockUserManager.GetUserManager(userList);
            var db = InitialDatabase.InitDatabase();
            IRefreshTokenService refreshToken = new RefreshTokenService(db, jwtHandler);
            var service = new UserService(manager.Object, jwtHandler, refreshToken);
            var command = new RegisterUserCommand()
            {
                firstName = "Test",
                lastName = "Name",
                email = "TestEmail@test.pl",
                password = "Test"
            };
            //Act
            try
            {
                await service.RegisterUser(command);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("User_Name_Already_Exist", ex.Message);
            }
        }
        [Fact]
        public async void RegisterUser_WhenCommandHasntPassword_Should_ThrowException()
        {
            //Arrange
            var userList = MockUserManager.GetUsersList();
            userList.Add(new AppUser() { UserName = "Test Name", Email = "TestEmail" });
            IJwtHandler jwtHandler = new JwtHandler(generalOptions);
            var manager = MockUserManager.GetUserManager(userList);
            var db = InitialDatabase.InitDatabase();
            IRefreshTokenService refreshToken = new RefreshTokenService(db, jwtHandler);
            var service = new UserService(manager.Object, jwtHandler, refreshToken);
            var command = new RegisterUserCommand()
            {
                firstName = "Again",
                lastName = "Test",
                email = "TestEmail@test.pl"
            };
            //Act
            try
            {
                await service.RegisterUser(command);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("Password_Cannot_Be_Null", ex.Message);
            }
        }
        [Fact]
        public async void RegisterUser_WhenCommandHaveNullEmail_Should_ThrowException()
        {
            //Arrange
            var userList = MockUserManager.GetUsersList();
            userList.Add(new AppUser() { UserName = "Test Name", Email = "TestEmail" });
            IJwtHandler jwtHandler = new JwtHandler(generalOptions);
            var manager = MockUserManager.GetUserManager(userList);
            var db = InitialDatabase.InitDatabase();
            IRefreshTokenService refreshToken = new RefreshTokenService(db, jwtHandler);
            var service = new UserService(manager.Object, jwtHandler, refreshToken);
            var command = new RegisterUserCommand()
            {
                password = "Test",
                firstName = "Again",
                lastName = "Test"
            };
            //Act
            try
            {
                await service.RegisterUser(command);
            }

            //Assert
            catch (Exception ex)
            {
                Assert.Equal("Email_Cannot_Be_Null", ex.Message);
            }
        }
        [Fact]
        public async void RegisterUser_WhenCommandIsCorrect_Should_CreateNewUser()
        {
            //Arrange
            var userList = MockUserManager.GetUsersList();
            IJwtHandler jwtHandler = new JwtHandler(generalOptions);
            var manager = MockUserManager.GetUserManager(userList);
            var db = InitialDatabase.InitDatabase();
            IRefreshTokenService refreshToken = new RefreshTokenService(db, jwtHandler);
            var service = new UserService(manager.Object, jwtHandler, refreshToken);
            var command = new RegisterUserCommand()
            {
                password = "Test",
                firstName = "Again",
                lastName = "Test",
                email = "TestEmail@test.pl"
            };

            //Act
            await service.RegisterUser(command);

            //Assert
            Assert.Equal(1, userList.Count());
        }
        [Fact]
        public async void RegisterUser_WhenCommandIsCorrect_Should_ReturnToken()
        {
            //Arrange
            var userList = MockUserManager.GetUsersList();
            IJwtHandler jwtHandler = new JwtHandler(generalOptions);
            var manager = MockUserManager.GetUserManager(userList);
            var db = InitialDatabase.InitDatabase();
            IRefreshTokenService refreshToken = new RefreshTokenService(db, jwtHandler);
            var service = new UserService(manager.Object, jwtHandler, refreshToken);
            var command = new RegisterUserCommand()
            {
                password = "Test",
                firstName = "Again",
                lastName = "Test",
                email = "TestEmail@test.pl"
            };

            //Act
            var result = await service.RegisterUser(command);

            //Assert
            Assert.False(string.IsNullOrWhiteSpace(result.jwtToken));
        }
        [Fact]
        public async void LoginUser_WhenCommandIsNull_Should_ThrowException()
        {
            //Arrange
            var userList = MockUserManager.GetUsersList();
            IJwtHandler jwtHandler = new JwtHandler(generalOptions);
            var manager = MockUserManager.GetUserManager(userList);
            var db = InitialDatabase.InitDatabase();
            IRefreshTokenService refreshToken = new RefreshTokenService(db, jwtHandler);
            var service = new UserService(manager.Object, jwtHandler, refreshToken);
            var command = new LoginUserCommand() { };
            //Act
            try
            {
                await service.LoginUser(null);
            }

            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Command_Is_Null", ex.Message);
            }
        }
        [Fact]
        public async void LoginUser_WhenUserDoesNotExist_Should_ThrowException()
        {
            //Arrange
            var userList = MockUserManager.GetUsersList();
            userList.Add(new AppUser() { Email = "Test" });
            IJwtHandler jwtHandler = new JwtHandler(generalOptions);
            var manager = MockUserManager.GetUserManager(userList);
            var db = InitialDatabase.InitDatabase();
            IRefreshTokenService refreshToken = new RefreshTokenService(db, jwtHandler);
            var service = new UserService(manager.Object, jwtHandler, refreshToken);
            var command = new LoginUserCommand() {email="TestEmail",password="Test" };
            //Act
            try
            {
                await service.LoginUser(command);
            }

            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Wrong_Credentials", ex.Message);
            }
        }
        [Fact]
        public async void LoginUser_WhenPasswordIsIncorect_Should_ThrowException()
        {
            //Arrange
            var userList = MockUserManager.GetUsersList();
            userList.Add(new AppUser() { Email = "Test" });
            IJwtHandler jwtHandler = new JwtHandler(generalOptions);
            var manager = MockUserManager.GetUserManager(userList);
            var db = InitialDatabase.InitDatabase();
            IRefreshTokenService refreshToken = new RefreshTokenService(db, jwtHandler);
            var service = new UserService(manager.Object, jwtHandler, refreshToken);
            var command = new LoginUserCommand() {email="Test",password="Test" };
            //Act
            try
            {
                await service.LoginUser(command);
            }

            //Assert
            catch(Exception ex)
            {
                Assert.Equal("Wrong_Credentials", ex.Message);
            }
        }
        [Fact]
        public async void LoginUser_WhenCommandIsCorrect_Should_ReturnToken()
        {
            //Arrange
            var userList = MockUserManager.GetUsersList();
            userList.Add(new AppUser() { Email = "Test",UserName = "Test Name" });
            IJwtHandler jwtHandler = new JwtHandler(generalOptions);
            var manager = MockUserManager.GetUserManager(userList);
            var db = InitialDatabase.InitDatabase();
            IRefreshTokenService refreshToken = new RefreshTokenService(db, jwtHandler);
            var service = new UserService(manager.Object, jwtHandler, refreshToken);
            var command = new LoginUserCommand() {email="Test",password="Test" };
            //Act
            var result = await service.LoginUser(command);

            //Assert
            Assert.False(string.IsNullOrWhiteSpace(result.jwtToken));
        }

    }
}
