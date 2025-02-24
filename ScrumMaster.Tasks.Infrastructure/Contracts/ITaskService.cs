using ScrumMaster.Tasks.Infrastructure.Commands;

namespace ScrumMaster.Tasks.Infrastructure.Contracts
{
    public interface ITaskService
    {
        Task<Guid> CreateTask(CreateTaskCommand command);
    }
}
