using Microsoft.Extensions.DependencyInjection;
using ScrumMaster.Sprints.Infrastructure.Contracts;
using ScrumMaster.Sprints.Infrastructure.DataAccess;
using ScrumMaster.Sprints.Infrastructure.Implementations;

namespace ScrumMaster.Sprints.Infrastructure
{
    public static class DependencyInfrastructureLayer
    {
        public static IServiceCollection AddInfratstructure(this IServiceCollection service)
        {
            service.AddScoped<ISprintService, SprintService>();
            service.AddScoped<ISprintDbContext, SprintDbContext>();
            return service;
        }
    }
}
