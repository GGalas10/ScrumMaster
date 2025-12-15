using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ScrumMaster.Tasks.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CommentController : Controller
    {
        protected Guid UserId => Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        [HttpGet]
        public async Task<IActionResult> GetTaskComment([FromQuery]Guid taskId)
        {
            return Ok();
        }
    }
}
