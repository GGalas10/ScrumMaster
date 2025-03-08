using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ScrumMaster.Sprints.Infrastructure.DataAccess;


namespace ScrumMaster.Sprints.Infrastructure.Tests.Commons
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(service =>
            {
                var descriptor = service.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<SprintDbContext>));
                if (descriptor != null)
                    service.Remove(descriptor);
                service.AddDbContext<SprintDbContext>(options =>
                {
                    options.UseInMemoryDatabase("SprintTestDb");
                });
                var sp = service.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SprintDbContext>();
            });
        }
    }
}
