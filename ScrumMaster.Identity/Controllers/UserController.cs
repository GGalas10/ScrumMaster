using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Identity.Infrastructure.Contracts;

namespace ScrumMaster.Identity.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("api/User/GetById")]
        public async Task<IActionResult> GetUserById([FromQuery] Guid userId)
        {
            try
            {
                var result = await _userService.GetUserById(userId);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
