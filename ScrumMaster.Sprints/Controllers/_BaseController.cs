using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ScrumMaster.Sprints.Controllers
{
    [Authorize]
    public class _BaseController : ControllerBase
    {
        protected Guid UserId => Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
        protected string UserFullName => User.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
    }
}
