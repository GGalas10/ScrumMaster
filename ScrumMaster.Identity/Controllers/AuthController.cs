using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Identity.Infrastructure.Commands;
using ScrumMaster.Identity.Infrastructure.Contracts;

namespace ScrumMaster.Identity.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRefreshTokenService _refreshTokenService;
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
                var jwt = await _userService.RegisterUser(command);
                SetRefreshTokenCookie(jwt.refreshToken);
                return Json(jwt.jwtToken);
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
                var jwt = await _userService.LoginUser(command);
                SetRefreshTokenCookie(jwt.refreshToken);
                return Json(jwt.jwtToken);
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
                SetRefreshTokenCookie(result.refreshToken);
                return Json(result.jwtToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private void SetRefreshTokenCookie(string refreshToken)
        {
            Response.Cookies.Append("RefreshToken",refreshToken,new CookieOptions() { Expires = DateTime.Now.AddDays(6),SameSite = SameSiteMode.Strict ,Secure = true, HttpOnly = true});
        }
    }
}
