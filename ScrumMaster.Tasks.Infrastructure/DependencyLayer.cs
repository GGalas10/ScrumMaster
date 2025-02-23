using Microsoft.Extensions.DependencyInjection;
using ScrumMaster.Tasks.Infrastructure.Contracts;
using ScrumMaster.Tasks.Infrastructure.DataAccess;
using ScrumMaster.Tasks.Infrastructure.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
