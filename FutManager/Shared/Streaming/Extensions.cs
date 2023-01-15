using Microsoft.Extensions.DependencyInjection;

namespace Shared.Streaming;

public static class Extensions
{
    public static IServiceCollection AddStreaming(this IServiceCollection services)
        => services
            .AddSingleton<IStreamPublisher, DefaultStreamPublisher>()
            .AddSingleton<IStreamSubscriber, DefaultStreamSubscriber>();
}