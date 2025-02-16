using Microsoft.EntityFrameworkCore;
using ScrumMaster.Sprints.Core.Models;

namespace ScrumMaster.Sprints.Infrastructure.DataAccess
{
    public interface ISprintDbContext
    {
        DbSet<Sprint> Sprints { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
