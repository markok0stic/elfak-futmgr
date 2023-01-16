using Shared.Redis;
using Shared.Redis.Streaming;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHttpContextAccessor()
    .AddHostedService<Worker>()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();