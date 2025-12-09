using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Tasks.Infrastructure.Commands;
using ScrumMaster.Tasks.Infrastructure.Contracts;
using ScrumMaster.Tasks.Infrastructure.Exceptions;
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
            try
            {
                command.createdById = UserId;
                var result = await _taskService.CreateTask(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                    return BadRequest(ex.Message);
                return StatusCode(500, "Something_Wrong");
            }
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
        {
            try
            {
                await _taskService.UpdateTask(command, UserId);
                return Ok();

            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                    return BadRequest(ex.Message);
                return StatusCode(500, "Something_Wrong");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteTask([FromQuery] Guid taskId)
        {
            try
            {
                await _taskService.DeleteTask(taskId, UserId);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                    return BadRequest(ex.Message);
                return StatusCode(500, "Something_Wrong");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetTaskById([FromQuery] Guid taskId)
        {
            try
            {
                var result = await _taskService.GetTaskById(taskId, UserId);
                return Ok(taskId);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                    return BadRequest(ex.Message);
                return StatusCode(500, "Something_Wrong");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSprintTasks([FromQuery] Guid sprintId)
        {
            try
            {
                var result = await _taskService.GetAllSprintTasks(sprintId, UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                    return BadRequest(ex.Message);
                return StatusCode(500, "Something_Wrong");
            }
        }
        [HttpGet]
        public IActionResult GetTasksStatuses()
        {
            try
            {
                var result = _taskService.GetTaskStatuses();
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                    return BadRequest(ex.Message);
                return StatusCode(500, "Something_Wrong");
            }
        }
    }
}
