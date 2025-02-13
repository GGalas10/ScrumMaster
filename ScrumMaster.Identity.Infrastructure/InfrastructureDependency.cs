using Microsoft.Extensions.DependencyInjection;
using ScrumMaster.Identity.Infrastructure.Contracts;
using ScrumMaster.Identity.Infrastructure.Implementations;

namespace ScrumMaster.Identity.Infrastructure
{
    public static class InfrastructureDependency
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection service) 
        {
            service.AddScoped<IJwtHandler, JwtHandler>();
            service.AddScoped<IUserService, UserService>();
            return service;
        }
    }
}
