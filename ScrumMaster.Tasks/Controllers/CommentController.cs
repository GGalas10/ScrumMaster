using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Tasks.Core.Exceptions;
using ScrumMaster.Tasks.Infrastructure.Commands;
using ScrumMaster.Tasks.Infrastructure.Contracts;
using System.Security.Claims;

namespace ScrumMaster.Tasks.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        protected Guid UserId => Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        [HttpGet]
        public async Task<IActionResult> GetTaskComment([FromQuery]Guid taskId)
        {
            try
            {
                var result = await _commentService.GetTaskComments(taskId, UserId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                if (ex is BadRequestException)
                    return BadRequest(ex.Message);
                return StatusCode(500, "Someting_Wrong");
            }
        }
        [HttpPost]
        public async Task<IActionResult> SendComment([FromBody]CreateCommentCommand command)
        {
            try
            {
                command.senderId = UserId;
                await _commentService.SendComment(command);
                return Ok();
            }
            catch(Exception ex)
            {
                if (ex is BadRequestException)
                    return BadRequest(ex.Message);
                return StatusCode(500, "Someting_Wrong");
            }
        }
    }
}
