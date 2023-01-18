using Aggregator;
using Shared.Redis;
using Shared.Redis.Streaming;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHttpContextAccessor()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming()
    .AddHostedService<Worker>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();