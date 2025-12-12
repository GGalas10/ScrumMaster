using ScrumMaster.Tasks.Infrastructure.Commands;
using ScrumMaster.Tasks.Infrastructure.DTOs;

namespace ScrumMaster.Tasks.Infrastructure.Contracts
{
    public interface ITaskService
    {
        Task<Guid> CreateTask(CreateTaskCommand command);
        Task UpdateTask(UpdateTaskCommand command, Guid userId);
        Task DeleteTask(Guid taskId, Guid userId);
        Task<TaskDTO> GetTaskById(Guid taskId, Guid userId);
        Task<List<TaskListDTO>> GetAllSprintTasks(Guid sprintId, Guid userId);
        List<TaskStatusDTO> GetTaskStatuses();
    }
}
