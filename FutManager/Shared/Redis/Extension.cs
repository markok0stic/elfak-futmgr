using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Shared.Redis
{
    /// <summary>
    /// Every extension method is used to inject exact dependencies for it
    /// </summary>
    public static class Extension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("Redis");
            var options = new RedisOptions();
            section.Bind(options);
            services
                .Configure<RedisOptions>(section)
                .AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options.Uri));
            return services;
        }
    }
}

