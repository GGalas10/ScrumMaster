using ScrumMaster.Identity.Infrastructure.Commands;
using ScrumMaster.Identity.Infrastructure.DTO;

namespace ScrumMaster.Identity.Infrastructure.Contracts
{
    public interface IUserService
    {
        Task<AuthDTO> RegisterUser(RegisterUserCommand command);
        Task<AuthDTO> LoginUser(LoginUserCommand command);
        Task<string> GetUserInfo(Guid userId);
    }
}
