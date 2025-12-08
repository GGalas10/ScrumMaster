using ScrumMaster.Tasks.Core.Enums;

namespace ScrumMaster.Tasks.Infrastructure.Contracts
{
    public interface IProjectAPIService
    {
        Task<bool> UserHavePremissions(Guid userId, Guid projectId, UserPremissionsEnum role);
    }
}
