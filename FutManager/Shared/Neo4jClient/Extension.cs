using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neo4jClient;

namespace Shared.Neo4jClient;

public static class Extension
{
    public static IServiceCollection AddNeo4J(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection("Neo4J");
        var options = new Neo4JOptions();
        section.Bind(options);
        services.Configure<Neo4JOptions>(section);
        var graphClient = new BoltGraphClient(options.Uri, options.Username, options.Password);
        graphClient.ConnectAsync();
        services.AddSingleton<IGraphClient>(graphClient);
        return services;
    }
}
