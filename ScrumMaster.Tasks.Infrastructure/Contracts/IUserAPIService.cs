using ScrumMaster.Tasks.Infrastructure.DTOs.Users;

namespace ScrumMaster.Tasks.Infrastructure.Contracts
{
    public interface IUserAPIService
    {
        Task<UserDTO> GetUserById(Guid userId);
    }
}
