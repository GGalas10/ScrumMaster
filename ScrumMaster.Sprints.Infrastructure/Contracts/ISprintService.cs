using ScrumMaster.Sprints.Infrastructure.Commands;

namespace ScrumMaster.Sprints.Infrastructure.Contracts
{
    public interface ISprintService
    {
        Task<Guid> CreateNewSprintAsync(CreateSprintCommand command);
        Task
    }
}
