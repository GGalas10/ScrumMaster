using Microsoft.EntityFrameworkCore;
using ScrumMaster.Project.Core.Models;

namespace ScrumMaster.Project.Infrastructure.DataAccesses
{
    public interface IProjectDbContext
    {
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<ProjectSettings> ProjectSettings { get; set; }
        public DbSet<ProjectUserAccess> ProjectUserAccesses { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
