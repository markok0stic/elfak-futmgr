using Microsoft.Extensions.DependencyInjection;

namespace Shared.Neo4j.DbService;

public static class Extension
{
    public static IServiceCollection AddNeo4JDbService(this IServiceCollection services)
    {
        return services.AddSingleton<IGraphDbService, GraphDbService>();
    }
}