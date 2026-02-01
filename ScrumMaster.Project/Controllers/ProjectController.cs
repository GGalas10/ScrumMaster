using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Project.CustomAttributes;
using ScrumMaster.Project.Infrastructure.Contracts;
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
            var boardInfo = await _projectService.GetBoardInfo(projectId, UserId);
            return Ok(boardInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAccess([FromBody] AccessCommand command)
        {
            command.adminId = UserId;
            await _accessService.AddUserAccess(command);
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUserAccessRole([FromBody] AccessCommand command)
        {
            command.adminId = UserId;
            await _accessService.UpdateUserAccess(command);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveUserAccess([FromBody] DeleteAccessCommand command)
        {
            command.adminId = UserId;
            await _accessService.RemoveUserAccess(command);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] AddProjectCommand command)
        {
            command.userId = UserId;
            var result = await _projectService.AddNewProject(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject([FromBody] UpdateProjectCommand command)
        {
            command.userId = UserId;
            await _projectService.UpdateProject(command);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProjectRole([FromQuery] Guid projectId, [FromQuery] Guid? userId = null)
        {
            var role = await _accessService.GetUserProjectRole(projectId, userId == null ? UserId : userId.Value);
            return Ok(role);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProjects()
        {
            var result = await _projectService.GetUsersProject(UserId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProjectMembers([FromQuery] Guid projectId)
        {
            var result = await _projectService.GetAllProjectMembers(projectId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> CanManageMembers([FromQuery] Guid projectId)
        {
            var result = await _accessService.CanManageMembers(projectId, UserId);
            return Ok(result);
        }
    }
}
