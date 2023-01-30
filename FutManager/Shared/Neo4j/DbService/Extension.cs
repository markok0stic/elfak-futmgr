using Microsoft.Extensions.DependencyInjection;
using Shared.Serialization;

namespace Shared.Neo4j.DbService;

public static class Extension
{
    public static IServiceCollection AddNeo4JDbService<T,TQ>(this IServiceCollection services) where T : class where TQ : class?
    {
        return services
            .AddCustomSerialization()
            .AddSingleton(typeof(IGraphDbService<T, TQ>), typeof(GraphDbService<T, TQ>));
    }
}