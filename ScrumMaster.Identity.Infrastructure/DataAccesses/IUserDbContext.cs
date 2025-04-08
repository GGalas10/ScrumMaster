using Microsoft.EntityFrameworkCore;
using ScrumMaster.Identity.Core.Models;

namespace ScrumMaster.Identity.Infrastructure.DataAccesses
{
    public interface IUserDbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<AppUser> Users { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
