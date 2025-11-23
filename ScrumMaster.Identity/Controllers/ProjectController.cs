using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Identity.Infrastructure.Contracts;

namespace ScrumMaster.Identity.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IUserService _userService;
        public ProjectController(IUserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        [HttpPost("api/Project/GetUserList")]
        public async Task<IActionResult> GetUserList([FromBody]List<Guid> userIds)
        {
            var result = await _userService.GetUsers(userIds);
            return Json(result);
        }
    }
}
