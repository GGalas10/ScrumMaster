using Microsoft.EntityFrameworkCore;
using ScrumMaster.Tasks.Core.Models;

namespace ScrumMaster.Tasks.Infrastructure.DataAccess
{
    public class TaskDbContext : DbContext, ITaskDbContext
    {
        public TaskDbContext(DbContextOptions options):base(options){}
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
