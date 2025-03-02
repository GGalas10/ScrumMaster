using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScrumMaster.Identity.Infrastructure.DTO;
using ScrumMaster.Identity.Infrastructure.Implementations;
using System.IdentityModel.Tokens.Jwt;

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
        [Fact]
        public void JwtHandler_WhenUserIsCorrect_Should_GiveTokenWithUserId()
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
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001").ToString(),
                Email = "Testemail",
                FirstName = "TestFirst",
                LastName = "TestLast"
            };
            var result = service.CreateToken(user);

            //Act

            var token = new JwtSecurityToken(result);

            //Assert
            Assert.Equal("00000000-0000-0000-0000-000000000001",token.Claims.First(x=>x.Type == JwtRegisteredClaimNames.Sub).Value);
        }
        [Fact]
        public void JwtHandler_WhenUserIsCorrect_Should_GiveTokenWithUserEmail()
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
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001").ToString(),
                Email = "Testemail",
                FirstName = "TestFirst",
                LastName = "TestLast"
            };
            var result = service.CreateToken(user);

            //Act

            var token = new JwtSecurityToken(result);

            //Assert
            Assert.Equal("Testemail", token.Claims.First(x=>x.Type == JwtRegisteredClaimNames.Email).Value);
        }
        [Fact]
        public void JwtHandler_WhenUserIsCorrect_Should_GiveTokenWithUserName()
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
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001").ToString(),
                Email = "Testemail",
                FirstName = "TestFirst",
                LastName = "TestLast"
            };
            var result = service.CreateToken(user);

            //Act

            var token = new JwtSecurityToken(result);

            //Assert
            Assert.Equal($"{user.FirstName} {user.LastName}", token.Claims.First(x=>x.Type == JwtRegisteredClaimNames.Name).Value);
        }
    }
}
