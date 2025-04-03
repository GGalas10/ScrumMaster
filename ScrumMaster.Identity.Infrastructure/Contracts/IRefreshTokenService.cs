using ScrumMaster.Identity.Infrastructure.DTO;

namespace ScrumMaster.Identity.Infrastructure.Contracts
{
    public interface IRefreshTokenService
    {
        Task<string> CreateRefreshToken(Guid userId);
        Task<AuthDTO> LoginWithRefresh(string refreshToken);
    }
}
