using ScrumMaster.Tasks.Handlers;

namespace ScrumMaster.Tasks.Extensions
{
    public static class HttpClientExtensions
    {
        public static IHttpClientBuilder AddApiClient(
        this IServiceCollection services,
        string name,
        string configKey)
        {
            return services.AddHttpClient(name, (sp, client) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                client.BaseAddress = new Uri(config[$"API:{configKey}"]);
            })
            .AddHttpMessageHandler<AccessTokenHandler>();
        }
    }
}
