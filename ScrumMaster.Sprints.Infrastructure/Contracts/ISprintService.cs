using ScrumMaster.Sprints.Infrastructure.Commands;
using ScrumMaster.Sprints.Infrastructure.DTO;

namespace ScrumMaster.Sprints.Infrastructure.Contracts
{
    public interface ISprintService
    {
        Task<Guid> CreateNewSprintAsync(CreateSprintCommand command, Guid userId, string userFullName);
        Task UpdateSprintAsync(UpdateSprintCommand command);
        Task DeleteSprintAsync(Guid id);
        Task<SprintDTO> GetSprintByIdAsync(Guid id);
        Task<List<SprintDTO>> GetAllUserSprintsAsync(Guid userId);
        Task<bool> CheckSprintExist(Guid sprintId);
    }
}
