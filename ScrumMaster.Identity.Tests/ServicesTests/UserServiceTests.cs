using Microsoft.Extensions.Options;
using ScrumMaster.Identity.Core.Models;
using ScrumMaster.Identity.Infrastructure.Contracts;
using ScrumMaster.Identity.Infrastructure.DTO;
using ScrumMaster.Identity.Infrastructure.Implementations;
using ScrumMaster.Identity.Tests.Common;

namespace ScrumMaster.Identity.Tests.ServicesTests
{
    public class UserServiceTests
    {
        IOptions<JwtSettings> generalOptions = Options.Create<JwtSettings>(new JwtSettings());
        [Fact]
        public async Task RegisterUser_WhenCommandNull_Should_ThrowException()
        {
            //Arrange
            var userList = MockUserManager.GetUsersList();
            IJwtHandler jwtHandler = new JwtHandler(generalOptions);
            var manager = MockUserManager.GetUserManager<AppUser>(userList);
            var service = new UserService(manager.Object,jwtHandler);
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
    }
}
