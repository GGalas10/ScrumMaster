using Microsoft.AspNetCore.Mvc;
using ScrumMaster.Identity.Infrastructure.Commands;
using ScrumMaster.Identity.Infrastructure.Contracts;

namespace ScrumMaster.Identity.Controllers
{
    public class AuthController :Controller
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/Register")]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterUserCommand command)
        {
            try
            {
                var jwt = await _userService.RegisterUser(command);
                return Ok(jwt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("/Login")]
        public async Task<IActionResult> LoginUser([FromBody]LoginUserCommand command)
        {
            try
            {
                var jwt = await _userService.LoginUser(command);
                return Ok(jwt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
