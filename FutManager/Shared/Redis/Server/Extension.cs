using Microsoft.Extensions.DependencyInjection;

namespace Shared.Redis.Server
{
    /// <summary>
    /// Every extension method is used to inject exact dependencies for it
    /// </summary>
    public static class Extension
    {
        public static IServiceCollection AddRedisServer(this IServiceCollection services)
        {
            return services.AddSingleton<IRedisServerClient, RedisServerClient>();
        }
    }
}

