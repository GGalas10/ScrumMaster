using Microsoft.EntityFrameworkCore;
using ScrumMaster.Tasks.Core.Enums;
using ScrumMaster.Tasks.Core.Models;
using ScrumMaster.Tasks.Infrastructure.Commands;
using ScrumMaster.Tasks.Infrastructure.Contracts;
using ScrumMaster.Tasks.Infrastructure.DataAccess;
using ScrumMaster.Tasks.Infrastructure.DTOs;
using ScrumMaster.Tasks.Infrastructure.DTOs.Users;
using ScrumMaster.Tasks.Infrastructure.Exceptions;

namespace ScrumMaster.Tasks.Infrastructure.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskDbContext _context;
        private readonly ISprintAPIService _sprintAPIService;
        private readonly IProjectAPIService _projectAPIService;
        private readonly IUserAPIService _userAPIService;
        public TaskService(ITaskDbContext context, ISprintAPIService sprintAPIService, IProjectAPIService projectAPIService, IUserAPIService userAPIService)
        {
            _context = context;
            _sprintAPIService = sprintAPIService;
            _projectAPIService = projectAPIService;
            _userAPIService = userAPIService;
        }
        public async Task<Guid> CreateTask(CreateTaskCommand command)
        {
            if (command == null)
                throw new BadRequestException("Command_Cannot_Be_Null");
            await _sprintAPIService.CheckSprintExist(command.sprintId);
            await UserHavePremissions(command.createdById, command.sprintId, UserPremissionsEnum.CanSave);
            var user = await _userAPIService.GetUserById(command.createdById);
            var newTask = new TaskModel(command.title, command.description, command.sprintId, command.createdById, $"{user.firstName} {user.lastName}", command.status);
            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();
            return newTask.Id;
        }
        public async Task UpdateTask(UpdateTaskCommand command, Guid userId)
        {
            if (command == null)
                throw new BadRequestException("Command_Cannot_Be_Null");
            var oldTask = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == command.oldTaskId);
            await _sprintAPIService.CheckSprintExist(command.sprintId);
            await UserHavePremissions(userId, command.sprintId, UserPremissionsEnum.CanSave);
            if (oldTask == null)
                throw new BadRequestException("Cannot_Find_Task_To_Update");
            UserDTO assignedUser = null;
            if (command.assignedUserId != Guid.Empty)
                assignedUser = await _userAPIService.GetUserById(command.assignedUserId);
            var anyChanges = oldTask.UpdateTask(command.title, command.description, command.status, command.sprintId, command.assignedUserId, assignedUser == null ? "" : $"{assignedUser.firstName} {assignedUser.lastName}");
            if (!anyChanges)
                throw new BadRequestException("Any_Changes_To_Change");
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTask(Guid taskId, Guid userId)
        {
            if (taskId == Guid.Empty)
                throw new BadRequestException("TaskId_Cannot_Be_Empty");
            var taskToDelete = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

            if (taskToDelete == null)
                throw new BadRequestException("Cannot_Find_Task_To_Delete");

            await UserHavePremissions(userId, taskToDelete.SprintId, UserPremissionsEnum.CanDelete);
            _context.Tasks.Remove(taskToDelete);
            var comments = await _context.Comments.Where(x => x.taskId == taskId).ToListAsync();
            if(comments != null && comments.Count() > 0)
                _context.Comments.RemoveRange(comments);
            await _context.SaveChangesAsync();
        }
        public async Task<TaskDTO> GetTaskById(Guid taskId, Guid userId)
        {
            if (taskId == Guid.Empty)
                throw new BadRequestException("TaskId_Cannot_Be_Empty");
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
            if (task == null)
                throw new BadRequestException("Cannot_Find_Task");
            await UserHavePremissions(userId, task.SprintId, UserPremissionsEnum.CanRead);
            var creatBy = await _userAPIService.GetUserById(task.CreateById);
            var assignedTo = await _userAPIService.GetUserById(task.CreateById);
            return TaskDTO.GetFromModel(task);
        }
        public async Task<List<TaskListDTO>> GetAllSprintTasks(Guid sprintId, Guid userId)
        {
            if (sprintId == Guid.Empty)
                throw new BadRequestException("SprintId_Cannot_Be_Empty");
            await UserHavePremissions(userId, sprintId, UserPremissionsEnum.CanRead);
            var tasks = await _context.Tasks.Where(x => x.SprintId == sprintId).ToListAsync();
            await UserHavePremissions(userId, sprintId, UserPremissionsEnum.CanRead);
            return tasks.Select(x => TaskListDTO.GetFromModel(x)).ToList();
        }
        public List<TaskStatusDTO> GetTaskStatuses()
        {
            return Enum.GetValues(typeof(StatusEnum))
                        .Cast<StatusEnum>()
                        .Where(x => x != StatusEnum.Unknown)
                        .Select(x => new TaskStatusDTO
                        {
                            statusOrder = (int)x,
                            statusName = x.ToString(),
                        })
                        .ToList();
        }
        public async Task UpdateTaskStatus(UpdateTaskStatusCommand command)
        {
            if(command == null)
                throw new BadRequestException("Command_Cannot_Be_Null");
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == command.taskId);
            if (task == null)
                throw new BadRequestException("Cannot_Find_Task");
            task.SetStatus(command.status);
            _context.Update(task);
            await _context.SaveChangesAsync();
        }
        private async Task UserHavePremissions(Guid userId, Guid sprintId, UserPremissionsEnum premission)
        {
            var projectId = await _sprintAPIService.GetProjectIdBySprintId(sprintId);
            var canAdd = await _projectAPIService.UserHavePremissions(userId, projectId, UserPremissionsEnum.CanSave);
        }
    }
}
