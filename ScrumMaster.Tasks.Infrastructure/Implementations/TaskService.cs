using ScrumMaster.Tasks.Core.Models;
using ScrumMaster.Tasks.Infrastructure.Commands;
using ScrumMaster.Tasks.Infrastructure.Contracts;
using ScrumMaster.Tasks.Infrastructure.DataAccess;

namespace ScrumMaster.Tasks.Infrastructure.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskDbContext _context;
        public TaskService(ITaskDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> CreateTask(CreateTaskCommand command)
        {
            var newTask = new TaskModel(command.title, command.description, command.assignedUserId, command.sprintId);
            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();
            return newTask.Id;
        }
    }
}
