using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Sprints.Infrastructure.Commands;
using ScrumMaster.Sprints.Infrastructure.Contracts;
using ScrumMaster.Sprints.Infrastructure.Exceptions;

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
        public async Task<IActionResult> CheckSprintExist([FromQuery]Guid sprintId)
        {
            var result = await _sprintService.CheckSprintExist(sprintId);
            return Ok(result);
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
                if(ex is BadRequestException)
                    return BadRequest(ex.Message);
                return StatusCode(500, "Something_Wrong");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetSprintsByProjectId([FromQuery] Guid projectId)
        {
            try
            {
                var result = await _sprintService.GetSprintsByProjectId(projectId, UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex is BadRequestException)
                    return BadRequest(ex.Message);
                return StatusCode(500, "Something_Wrong");
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
                if (ex is BadRequestException)
                    return BadRequest(ex.Message);
                return StatusCode(500, "Something_Wrong");
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSprint([FromBody]UpdateSprintCommand command)
        {
            try
            {
                await _sprintService.UpdateSprintAsync(command, UserId);
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
        public async Task<IActionResult> DeleteSprint([FromQuery]Guid sprintId)
        {
            try
            {
                await _sprintService.DeleteSprintAsync(sprintId, UserId);
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
        public async Task<IActionResult> GetActualSprint([FromQuery]Guid projectId)
        {
            try
            {
                var result = await _sprintService.GetActualSprintByProjectId(projectId,UserId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                if (ex is BadRequestException)
                    return BadRequest(ex.Message);
                return StatusCode(500, "Something_Wrong");
            }
        }
    }
}
