using ScrumMaster.Identity.Core.Models;

namespace ScrumMaster.Identity.Infrastructure.Contracts
{
    public interface IJwtHandler
    {
        string CreateToken(AppUser user);
    }
}
