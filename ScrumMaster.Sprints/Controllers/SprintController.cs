using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Sprints.Infrastructure.Commands;
using ScrumMaster.Sprints.Infrastructure.Contracts;

namespace ScrumMaster.Sprints.Controllers
{
    public class SprintController : _BaseController
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
        [HttpPost]
        public async Task<IActionResult> CreateSprint([FromBody]CreateSprintCommand command)
        {
            command.CreatedUserId = this.UserId;
            command.CreatedBy = this.UserFullName;
            var result = await _sprintService.CreateNewSprintAsync(command);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSprint([FromBody]UpdateSprintCommand command)
        {
            await _sprintService.UpdateSprintAsync(command);
            return Ok(); 
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteSprint([FromQuery]Guid sprintId)
        {
            await _sprintService.DeleteSprintAsync(sprintId);
            return Ok();
        }
    }
}
