using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Tasks.Infrastructure.Commands;
using ScrumMaster.Tasks.Infrastructure.Contracts;

namespace ScrumMaster.Tasks.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody]CreateTaskCommand command)
        {
            var result = await _taskService.CreateTask(command);
            return Ok(result);
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateTask([FromBody]UpdateTaskCommand command)
        {
            await _taskService.UpdateTask(command);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteTask([FromQuery]Guid taskId)
        {
            await _taskService.DeleteTask(taskId);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetTaskById([FromQuery]Guid taskId)
        {
            var result = await _taskService.GetTaskById(taskId);
            return Ok(taskId);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSprintTasks([FromQuery]Guid sprintId)
        {
            var result = await _taskService.GetAllSprintTasks(sprintId);
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
