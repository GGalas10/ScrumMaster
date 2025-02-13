using ScrumMaster.Identity.Infrastructure.Commands;

namespace ScrumMaster.Identity.Infrastructure.Contracts
{
    public interface IUserService
    {
        Task<string> RegisterUser(RegisterUserCommand command);
        Task<string> LoginUser(LoginUserCommand command);
    }
}
