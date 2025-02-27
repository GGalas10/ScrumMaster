using Microsoft.EntityFrameworkCore;
using ScrumMaster.Tasks.Core.Models;
using ScrumMaster.Tasks.Infrastructure.Commands;
using ScrumMaster.Tasks.Infrastructure.Contracts;
using ScrumMaster.Tasks.Infrastructure.DataAccess;
using ScrumMaster.Tasks.Infrastructure.DTOs;
using ScrumMaster.Tasks.Infrastructure.Exceptions;

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
            if (command == null)
                throw new BadRequestException("Command_Cannot_Be_Null");
            var newTask = new TaskModel(command.title, command.description, command.assignedUserId, command.sprintId);
            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();
            return newTask.Id;
        }
        public async Task UpdateTask(UpdateTaskCommand command)
        {
            if (command == null)
                throw new BadRequestException("Command_Cannot_Be_Null");
            var oldTask = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == command.oldTaskId);
            if (oldTask == null)
                throw new BadRequestException("Cannot_Find_Task_To_Update");
            var anyChanges = oldTask.UpdateTask(command.title, command.description, command.status, command.sprintId, command.assignedUserId);
            if (anyChanges)
                await _context.SaveChangesAsync();
            throw new BadRequestException("Any_Changes_To_Change");
        }
        public async Task DeleteTask(Guid taskId)
        {
            if (taskId == Guid.Empty)
                throw new BadRequestException("TaskId_Cannot_Be_Empty");
            
            var taskToDelete = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
            if (taskToDelete == null)
                throw new BadRequestException("Cannot_Find_Task_To_Delete");
            _context.Tasks.Remove(taskToDelete);
            await _context.SaveChangesAsync();
        }
        public async Task<TaskDTO> GetTaskById(Guid taskId)
            => TaskDTO.GetFromModel(await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId));
        public async Task<List<TaskDTO>> GetAllSprintTasks(Guid sprintId)
        {
            if (sprintId == Guid.Empty)
                throw new BadRequestException("SprintId_Cannot_Be_Empty");

            var tasks = await _context.Tasks.Where(x => x.SprintId == sprintId).ToListAsync();

            return tasks.Select(x => TaskDTO.GetFromModel(x)).ToList();
        }
    }
}
