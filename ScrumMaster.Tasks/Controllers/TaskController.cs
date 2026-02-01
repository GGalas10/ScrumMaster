using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Tasks.Infrastructure.Commands;
using ScrumMaster.Tasks.Infrastructure.Contracts;
using System.Security.Claims;

namespace ScrumMaster.Tasks.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        protected Guid UserId => Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
        {
            command.createdById = UserId;
            var result = await _taskService.CreateTask(command);
            return Ok(result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
        {
            await _taskService.UpdateTask(command, UserId);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTaskStatus([FromBody] UpdateTaskStatusCommand command)
        {
            await _taskService.UpdateTaskStatus(command);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask([FromQuery] Guid taskId)
        {
            await _taskService.DeleteTask(taskId, UserId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskById([FromQuery] Guid taskId)
        {
            var result = await _taskService.GetTaskById(taskId, UserId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSprintTasks([FromQuery] Guid sprintId)
        {
            var result = await _taskService.GetAllSprintTasks(sprintId, UserId);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetTasksStatuses()
        {
            var result = _taskService.GetTaskStatuses();
            return Ok(result);
        }
    }
}
