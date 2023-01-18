

using Microsoft.Extensions.DependencyInjection;

namespace Shared.Redis.Server;

public static class Extension
{
    public static IServiceCollection AddRedisServer(this IServiceCollection services)
    {
        return services.AddSingleton<IRedisDbClient, RedisDbClient>();
    }
}