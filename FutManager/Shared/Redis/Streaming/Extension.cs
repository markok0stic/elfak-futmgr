using Microsoft.Extensions.DependencyInjection;
using Shared.Streaming;

namespace Shared.Redis.Streaming;

/// <summary>
/// Every extension method is used to inject exact dependencies for it
/// </summary>
public static class Extension
{
    public static IServiceCollection AddRedisStreaming(this IServiceCollection services)
    {
       return services
           .AddSingleton<IStreamPublisher, RedisStreamPublisher>()
           .AddSingleton<IStreamSubscriber, RedisStreamSubscriber>();
    }
}