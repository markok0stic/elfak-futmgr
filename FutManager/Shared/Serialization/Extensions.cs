using Microsoft.Extensions.DependencyInjection;
using Shared.Serialization.Options;

namespace Shared.Serialization;

public static class Extensions
{
    public static IServiceCollection AddCustomSerialization(this IServiceCollection services,
        CustomJsonSerializerOptions? options = null)
    {
        return services
            .AddSingleton<CustomJsonSerializerOptions>(options ?? new CustomJsonSerializerOptions())
            .AddSingleton<ISerializer, CustomJsonSerializer>();
    }
}