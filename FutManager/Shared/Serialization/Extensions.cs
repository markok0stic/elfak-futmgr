using Microsoft.Extensions.DependencyInjection;
using Shared.Serialization.Options;

namespace Shared.Serialization;

/// <summary>
/// Every extension method is used to inject exact dependencies for it
/// </summary>
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