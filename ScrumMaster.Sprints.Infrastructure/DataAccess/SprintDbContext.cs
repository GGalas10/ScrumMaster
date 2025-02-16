using Microsoft.EntityFrameworkCore;
using ScrumMaster.Sprints.Core.Models;

namespace ScrumMaster.Sprints.Infrastructure.DataAccess
{
    public class SprintDbContext : DbContext, ISprintDbContext
    {
        public SprintDbContext(DbContextOptions<SprintDbContext> options) : base(options) { }
        public DbSet<Sprint> Sprints { get; set; }

    }
}
