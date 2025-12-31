using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Project.CustomAttributes;
using ScrumMaster.Project.Infrastructure.Contracts;
using ScrumMaster.Project.Infrastructure.CustomExceptions;
using ScrumMaster.Project.Infrastructure.DTOs.Commands;

namespace ScrumMaster.Project.Controllers
{
    [Authorize, InitUserId]
    [Route("[action]")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IAccessService _accessService;
        public Guid UserId { get; set; } = Guid.Empty;
        public ProjectController(IProjectService projectService, IAccessService accessService)
        {
            _projectService = projectService;
            _accessService = accessService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBoardInfo([FromQuery] Guid projectId)
        {
            try
            {
                var boardInfo = await _projectService.GetBoardInfo(projectId, UserId);
                return Ok(boardInfo);
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    RoleException => Forbid(),
                    _ => StatusCode(500, "Something_Went_Wrong"),
                };
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddUserAccess([FromBody] AccessCommand command)
        {
            try
            {
                command.adminId = UserId;
                await _accessService.AddUserAccess(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    RoleException => Forbid(),
                    _ => StatusCode(500, "Something_Went_Wrong"),
                };
            }
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateUserAccessRole([FromBody] AccessCommand command)
        {
            try
            {
                command.adminId = UserId;
                await _accessService.UpdateUserAccess(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    RoleException => Forbid(),
                    _ => StatusCode(500, "Something_Went_Wrong"),
                };
            }
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveUserAccess([FromBody] DeleteAccessCommand command)
        {
            try
            {
                command.adminId = UserId;
                await _accessService.RemoveUserAccess(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    RoleException => Forbid(),
                    _ => StatusCode(500, "Something_Went_Wrong"),
                };
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] AddProjectCommand command)
        {
            try
            {
                command.userId = UserId;
                var result = await _projectService.AddNewProject(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    RoleException => Forbid(),
                    _ => StatusCode(500, "Something_Went_Wrong"),
                };
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectCommand command)
        {
            try
            {
                command.userId = UserId;
                await _projectService.UpdateProject(command);
                return Ok();
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    RoleException => Forbid(),
                    _ => StatusCode(500, "Something_Went_Wrong"),
                };
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetUserProjectRole([FromQuery] Guid projectId, [FromQuery] Guid? userId = null)
        {
            try
            {
                var role = await _accessService.GetUserProjectRole(projectId, userId == null ? UserId : userId.Value);
                return Ok(role);
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    RoleException => Forbid(),
                    _ => StatusCode(500, "Something_Went_Wrong"),
                };
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetUserProjects()
        {
            try
            {
                var result = await _projectService.GetUsersProject(UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    RoleException => Forbid(),
                    _ => StatusCode(500, "Something_Went_Wrong"),
                };
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProjectMembers([FromQuery] Guid projectId)
        {
            try
            {
                var result = await _projectService.GetAllProjectMembers(projectId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return ex switch
                {
                    BadRequestException => BadRequest(ex.Message),
                    NotFoundException => NotFound(ex.Message),
                    RoleException => Forbid(),
                    _ => StatusCode(500, "Something_Went_Wrong"),
                };
            }
        }
        [HttpGet]
        public async Task<IActionResult> CanManageMembers([FromQuery]Guid projectId)
        {
            try
            {
                var result = await _accessService.CanManageMembers(projectId, UserId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return Ok(false);
            }
        }
    }
}
