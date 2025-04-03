using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScrumMaster.Identity.Core.Models;

namespace ScrumMaster.Identity.Infrastructure.DataAccesses
{
    public class UserDbContext : IdentityDbContext<AppUser>,IUserDbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
