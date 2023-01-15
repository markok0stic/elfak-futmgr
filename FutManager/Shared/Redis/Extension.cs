using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Shared.Redis;

public static class Extension
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Redis");
        var options = new RedisOptions();
        section.Bind(options);
        services.Configure<RedisOptions>(section);
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options.Uri));
        return services;
    }
}