using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Tasks.Infrastructure.Commands;

namespace ScrumMaster.Tasks.Controllers
{
    public class TaskController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody]CreateTaskCommand command)
        {
            return Ok();
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateTask([FromBody]UpdateTaskCommand command)
        {
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetTaskById([FromQuery]Guid taskId)
        {
            return Ok();
        }
    }
}
