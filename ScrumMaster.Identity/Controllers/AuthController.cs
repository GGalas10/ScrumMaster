using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Identity.Core.Models;
using ScrumMaster.Identity.Infrastructure.Commands;
using ScrumMaster.Identity.Infrastructure.Contracts;
using ScrumMaster.Identity.Infrastructure.DTO;
using System.Security.Claims;

namespace ScrumMaster.Identity.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
        public Guid UserId => Guid.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        public AuthController(IUserService userService, IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("/Register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            try
            {
                var result = await _userService.RegisterUser(command);
                SetTokens(result);
                return Json(result.userName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("/Login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand command)
        {
            try
            {
                var result = await _userService.LoginUser(command);
                SetTokens(result);
                return Json(result.userName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("/Refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["RefreshToken"];
                if (refreshToken == null)
                    return Unauthorized();
                var result = await _refreshTokenService.LoginWithRefresh(refreshToken);
                SetTokens(result);
                return Json(result.userName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                var result = await _userService.GetUserInfo(UserId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpGet("/Logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("RefreshToken");
            Response.Cookies.Delete("AccessToken");
            return Ok();
        }
        [AllowAnonymous]
        [HttpGet("/HealthCheck")]
        public IActionResult HealthCheck()
        {
            return Ok();
        }
        private void SetTokens(AuthDTO jwt)
        {
            Response.Cookies.Delete("RefreshToken");
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Append("RefreshToken", jwt.refreshToken, new CookieOptions() { Expires = DateTime.Now.AddDays(6), SameSite = SameSiteMode.Strict, Secure = true, HttpOnly = true });
            Response.Cookies.Append("AccessToken", jwt.jwtToken, new CookieOptions() { Expires = DateTime.Now.AddMinutes(1), SameSite = SameSiteMode.Strict, Secure = true, HttpOnly = true });
        }
    }
}
