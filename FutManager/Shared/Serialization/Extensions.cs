using Microsoft.Extensions.DependencyInjection;

namespace Shared.Serialization;

public static class Extensions
{
    public static IServiceCollection AddCustomSerialization(this IServiceCollection services)
    {
        return services.AddSingleton<ISerializer, CustomJsonSerializer>();
    }
}