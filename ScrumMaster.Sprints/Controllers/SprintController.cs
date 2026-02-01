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

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> CheckSprintExist([FromQuery] Guid sprintId)
        {
            var result = await _sprintService.CheckSprintExist(sprintId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSprintById([FromQuery] Guid sprintId)
        {
            var result = await _sprintService.GetSprintByIdAsync(sprintId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSprintsByProjectId([FromQuery] Guid projectId)
        {
            var result = await _sprintService.GetSprintsByProjectId(projectId, UserId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSprint([FromBody] CreateSprintCommand command)
        {
            var result = await _sprintService.CreateNewSprintAsync(command, UserId, UserFullName);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSprint([FromBody] UpdateSprintCommand command)
        {
            await _sprintService.UpdateSprintAsync(command, UserId);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSprint([FromQuery] Guid sprintId)
        {
            await _sprintService.DeleteSprintAsync(sprintId, UserId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetActualSprint([FromQuery] Guid projectId)
        {
            var result = await _sprintService.GetActualSprintByProjectId(projectId, UserId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectId([FromQuery] Guid sprintId)
        {
            var result = await _sprintService.GetProjectIdBySprintId(sprintId);
            return Ok(result);
        }
    }
}
