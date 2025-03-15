using Microsoft.Extensions.DependencyInjection;
using ScrumMaster.Tasks.Infrastructure.Contracts;
using ScrumMaster.Tasks.Infrastructure.DataAccess;
using ScrumMaster.Tasks.Infrastructure.Implementations;

namespace ScrumMaster.Tasks.Infrastructure
{
    public static class DependencyLayer
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskDbContext, TaskDbContext>();
            return services;
        }
    }
}
