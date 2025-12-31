using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Identity.Infrastructure.Contracts;
using System.Security.Claims;

namespace ScrumMaster.Identity.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public Guid UserId => Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
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
        [HttpPost]
        public async Task<IActionResult> GetUserByIdsList([FromBody] List<Guid> userIds)
        {
            try
            {
                var result = await _userService.GetUsers(userIds);
                return Json(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("api/User/FindUsers")]
        public async Task<IActionResult> FindUsers([FromQuery]string filter)
        {
            try
            {
                var result = await _userService.FindUsers(filter.Trim(),UserId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
