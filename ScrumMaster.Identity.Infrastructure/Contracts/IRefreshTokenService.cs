using ScrumMaster.Identity.Infrastructure.DTO;

namespace ScrumMaster.Identity.Infrastructure.Contracts
{
    internal interface IRefreshTokenService
    {
        Task<string> CreateRefreshToken();
        Task<AuthDTO> LoginWithRefresh(string refreshToken);
    }
}
