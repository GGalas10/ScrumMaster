using ScrumMaster.Project.Core.Enums;
using ScrumMaster.Project.Infrastructure.DTOs.Commands;

namespace ScrumMaster.Project.Infrastructure.Contracts
{
    public interface IAccessService
    {
        Task CreateUserRole(AccessCommand command);
        Task AddUserAccess(AccessCommand command);
        Task UpdateUserAccess(AccessCommand command);
        Task RemoveUserAccess(DeleteAccessCommand command);
        Task<ProjectRoleEnum> GetUserProjectRole(Guid projectId, Guid userId);
    }
}
