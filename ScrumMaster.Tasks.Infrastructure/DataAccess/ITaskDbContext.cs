using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ScrumMaster.Tasks.Core.Models;

namespace ScrumMaster.Tasks.Infrastructure.DataAccess
{
    public interface ITaskDbContext
    {
        DbSet<TaskModel> Tasks { get; set; }
        DbSet<Comment> Comments { get; set; }
        ChangeTracker ChangeTracker { get; }
        DatabaseFacade Database { get; }
        EntityEntry Entry(object entity);
        EntityEntry Add(object entity);
        void AddRange(params object[] entities);
        Task AddRangeAsync(params object[] entities);
        EntityEntry Update(object entity);
        void UpdateRange(params object[] entities);
        EntityEntry Attach(object entity);
        void AttachRange(params object[] entities);
        EntityEntry Remove(object entity);
        void RemoveRange(params object[] entities);
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
