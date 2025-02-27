using ScrumMaster.Tasks.Infrastructure.Commands;
using ScrumMaster.Tasks.Infrastructure.DTOs;

namespace ScrumMaster.Tasks.Infrastructure.Contracts
{
    public interface ITaskService
    {
        Task<Guid> CreateTask(CreateTaskCommand command);
        Task UpdateTask(UpdateTaskCommand command);
        Task DeleteTask(Guid taskId);
        Task<TaskDTO> GetTaskById(Guid taskId);
        Task<List<TaskDTO>> GetAllSprintTasks(Guid sprintId);
    }
}
