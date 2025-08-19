using Microsoft.EntityFrameworkCore;
using ScrumMaster.Project.Core.Models;

namespace ScrumMaster.Project.Infrastructure.DataAccesses
{
    public class ProjectDbContext : DbContext, IProjectDbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options) { }
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<ProjectSettings> ProjectSettings { get; set; }
        public DbSet<ProjectUserAccess> ProjectUserAccesses { get; set; }
    }
}
