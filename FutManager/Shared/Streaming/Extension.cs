using Microsoft.Extensions.DependencyInjection;

namespace Shared.Streaming
{
    public static class Extension
    {
        public static IServiceCollection AddStreaming(this IServiceCollection services)
        {
            return services
                .AddSingleton<IStreamPublisher, DefaultStreamPublisher>()
                .AddSingleton<IStreamSubscriber, DefaultStreamSubscriber>();
        }
    }
}

