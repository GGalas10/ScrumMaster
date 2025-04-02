using Microsoft.EntityFrameworkCore;
using ScrumMaster.Identity.Core.Models;
using ScrumMaster.Identity.Infrastructure.Contracts;
using ScrumMaster.Identity.Infrastructure.DataAccess;
using ScrumMaster.Identity.Infrastructure.DTO;
using System.Security.Cryptography;
using System.Text;

namespace ScrumMaster.Identity.Infrastructure.Implementations
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly UserDbContext _userDbContext;
        private readonly IJwtHandler _jwtHandler;
        public RefreshTokenService(UserDbContext userDbContext, IJwtHandler jwtHandler)
        {
            _userDbContext = userDbContext;
            _jwtHandler = jwtHandler;
        }

        public async Task<string> CreateRefreshToken(Guid userId)
        {
            var newRefresh = GenerateRefreshToken();
            _userDbContext.RefreshTokens.Add(new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = newRefresh,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            });
            await _userDbContext.SaveChangesAsync();
            return newRefresh;
        }

        public async Task<AuthDTO> LoginWithRefresh(string refreshToken)
        {
            var oldRefresh = _userDbContext.RefreshTokens.Include(x=>x.User).FirstOrDefault(x => !x.IsRevoked && x.Token == refreshToken);
            if (oldRefresh == null)
                throw new UnauthorizedAccessException("Cannot_Find_RefreshToken");
            var newJwt = _jwtHandler.CreateToken(oldRefresh.User);     
            var newRefresh = await CreateRefreshToken(oldRefresh.UserId);
            _userDbContext.Remove(oldRefresh);
            _userDbContext.SaveChanges();
            return new AuthDTO()
            {
                jwtToken = newJwt,
                refreshToken = refreshToken
            };
        }
        private static string GenerateRefreshToken()
        {
            const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder newToken = new StringBuilder();
            var length = new Random().Next(36,50);
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[length];
                rng.GetBytes(data);
                for (int i = 0; i < length; i++)
                {
                    newToken.Append(_chars[data[i] % _chars.Length]);
                }
            }
            return newToken.ToString();
        }
    }
}
