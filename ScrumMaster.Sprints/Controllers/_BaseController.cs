using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ScrumMaster.Sprints.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class _BaseController : ControllerBase
    {
        protected Guid UserId => Guid.Parse(User.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier)?.Value);
        protected string UserFullName => User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
    }
}
