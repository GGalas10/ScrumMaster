using Microsoft.Extensions.Options;
using ScrumMaster.Identity.Infrastructure.DTO;
using ScrumMaster.Identity.Infrastructure.Implementations;

namespace ScrumMaster.Identity.Tests.ServicesTests
{
    public class IdentityTests
    {
        [Fact]
        public void JwtHandler_WhenGetNull_Should_ThrowException()
        {
            //Arrange
            var service = new JwtHandler(Options.Create<JwtSettings>(new JwtSettings()));


            //Act
            try
            {
                service.CreateToken(null);
            }

            //Assert
            catch(Exception ex)
            {
                Assert.Equal("User_Cannot_Be_Null", ex.Message);
            }
        }
        [Fact]
        public void JwtHandler_WhenUserIsCorrect_Should_ReturnJwtToken()
        {
            //Arrange
            var options = Options.Create<JwtSettings>(new JwtSettings()
            {
                ExpirationMinutes = 1,
                secret = "secrasdas12312312ergdfsasddasdassdasdet"
            });
            var service = new JwtHandler(options);
            var user = new Core.Models.AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "Testemail",
                FirstName = "TestFirst",
                LastName = "TestLast"
            };

            //Act
            var result = service.CreateToken(user);

            //Assert
            Assert.False(string.IsNullOrEmpty(result));
        }
    }
}
