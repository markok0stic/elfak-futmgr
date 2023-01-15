using Microsoft.Extensions.DependencyInjection;
using Shared.Streaming;

namespace Shared.Redis.Streaming;

public static class Extensions
{
    public static IServiceCollection AddRedisStreaming(this IServiceCollection services)
    {
       return services
           .AddSingleton<IStreamPublisher, RedisStreamPublisher>()
           .AddSingleton<IStreamSubscriber, RedisStreamSubscriber>();
    }
}