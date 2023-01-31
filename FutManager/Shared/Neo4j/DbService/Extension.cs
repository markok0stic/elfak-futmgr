using Microsoft.Extensions.DependencyInjection;
using Shared.Serialization;

namespace Shared.Neo4j.DbService;
/// <summary>
/// Every extension method is used to inject exact dependencies for it
/// </summary>
public static class Extension
{
    public static IServiceCollection AddNeo4JDbService<T,TQ>(this IServiceCollection services) where T : class where TQ : class?
    {
        return services
            .AddCustomSerialization()
            .AddSingleton(typeof(IGraphDbService<T, TQ>), typeof(GraphDbService<T, TQ>));
    }
}