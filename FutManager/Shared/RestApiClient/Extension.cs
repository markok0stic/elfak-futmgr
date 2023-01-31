using Microsoft.Extensions.DependencyInjection;

namespace Shared.RestApiClient;

/// <summary>
/// Every extension method is used to inject exact dependencies for it
/// </summary>
public static class Extension
{
    public static IServiceCollection AddRestApiClient(this IServiceCollection services)
    {
        return services.AddSingleton<IApiClient, ApiClient>();
    }
}