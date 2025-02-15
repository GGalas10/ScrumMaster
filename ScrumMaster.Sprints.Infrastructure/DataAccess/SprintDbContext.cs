using Microsoft.EntityFrameworkCore;
using ScrumMaster.Sprints.Core.Models;

namespace ScrumMaster.Sprints.Infrastructure.DataAccess
{
    public class SprintDbContext : DbContext
    {
        public SprintDbContext(DbContextOptions<SprintDbContext> options) : base(options) { }
        DbSet<Sprint> Sprints { get; set; } 
    }
}
