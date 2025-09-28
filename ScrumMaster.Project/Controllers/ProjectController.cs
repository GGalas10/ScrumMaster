using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Project.CustomAttributes;
using ScrumMaster.Project.Infrastructure.Contracts;
using ScrumMaster.Project.Infrastructure.CustomExceptions;
using ScrumMaster.Project.Infrastructure.DTOs.Commands;
using System.IdentityModel.Tokens.Jwt;

namespace ScrumMaster.Project.Controllers
{
    [Authorize, InitUserId]
    [Route("[action]")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IAccessService _accessService;
        public Guid UserId { get; set; }
        public ProjectController(IProjectService projectService,IAccessService accessService)
        {
            _projectService = projectService;
            _accessService = accessService;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult PeekToken()
        {
            var auth = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(auth) || !auth.StartsWith("Bearer "))
                return BadRequest("Brak nagłówka Authorization: Bearer <token>");

            var token = auth.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token)) return BadRequest("Nieprawidłowy format JWT");

            var jwt = handler.ReadJwtToken(token);
            var header = new
            {
                alg = jwt.Header.Alg,
                kid = jwt.Header.TryGetValue("kid", out var kidObj) ? kidObj?.ToString() : null
            };
            var payload = new
            {
                iss = jwt.Issuer,
                aud = jwt.Audiences,     // często lista!
                exp = jwt.Payload.Exp,   // unix ts
                nbf = jwt.Payload.Nbf,
                iat = jwt.Payload.Iat,
                claims = jwt.Claims.Select(c => new { c.Type, c.Value })
            };

            return Ok(new { header, payload });
        }
        [HttpGet]
        public async Task<IActionResult> GetBoardInfo([FromQuery]Guid projectId)
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
        public async Task<IActionResult> GetUserProjectRole([FromQuery] Guid projectId)
        {
            try
            {
                var role = await _accessService.GetUserProjectRole(projectId, UserId);
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
    }
}
