using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Sprints.Infrastructure.Contracts;

namespace ScrumMaster.Sprints.Controllers
{
    public class SprintController : Controller
    {
        private readonly ISprintService _sprintService;
        public SprintController(ISprintService sprintService)
        {
            _sprintService = sprintService;
        }
        [HttpGet]
        public async Task<IActionResult> GetSprintById([FromQuery] Guid sprintId)
        {
            var result = await _sprintService.GetSprintByIdAsync(sprintId);
            return Ok(result);
        }
    }
}
