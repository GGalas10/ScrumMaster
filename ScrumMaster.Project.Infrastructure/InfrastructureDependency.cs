using Microsoft.Extensions.DependencyInjection;
using ScrumMaster.Project.Infrastructure.Contracts;
using ScrumMaster.Project.Infrastructure.DataAccesses;
using ScrumMaster.Project.Infrastructure.Implementations;

namespace ScrumMaster.Project.Infrastructure
{
    public static class InfrastructureDependency
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection service)
        {
            service.AddScoped<IProjectDbContext, ProjectDbContext>();
            service.AddScoped<IProjectService, ProjectService>();
            service.AddScoped<IAccessService, AccessService>();
            return service;
        }
    }
}
