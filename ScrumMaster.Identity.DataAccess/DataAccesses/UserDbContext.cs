using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScrumMaster.Identity.Core.Models;

namespace ScrumMaster.Identity.Infrastructure.DataAccess
{
    public class UserDbContext : IdentityDbContext<AppUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
    }
}
