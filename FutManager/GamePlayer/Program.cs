using GamePlayer;
using Shared.Redis;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => 
    { services
        .AddRedis(hostContext.Configuration)
        .AddHostedService<Worker>(); 
    })
    .Build();

await host.RunAsync();