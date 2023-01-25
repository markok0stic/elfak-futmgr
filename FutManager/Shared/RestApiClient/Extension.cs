using Microsoft.Extensions.DependencyInjection;

namespace Shared.RestApiClient;

public static class Extension
{
    public static IServiceCollection AddRestApiClient(this IServiceCollection services)
    {
        return services.AddSingleton<IApiClient, ApiClient>();
    }
}