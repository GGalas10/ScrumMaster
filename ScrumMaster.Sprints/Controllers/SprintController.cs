using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Sprints.Infrastructure.Commands;
using ScrumMaster.Sprints.Infrastructure.Contracts;

namespace ScrumMaster.Sprints.Controllers
{
    [Route("{controller}/{action}")]
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
            try
            {
                var result = await _sprintService.GetSprintByIdAsync(sprintId);
                return Ok(result);
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateSprint([FromBody]CreateSprintCommand command)
        {
            try 
            { 
                var result = await _sprintService.CreateNewSprintAsync(command, UserId, UserFullName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSprint([FromBody]UpdateSprintCommand command)
        {
            try
            {
                await _sprintService.UpdateSprintAsync(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteSprint([FromQuery]Guid sprintId)
        {
            try
            {
                await _sprintService.DeleteSprintAsync(sprintId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
