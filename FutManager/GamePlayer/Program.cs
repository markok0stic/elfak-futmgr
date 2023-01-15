using GamePlayer.Matchmaking.Requests;
using GamePlayer.Matchmaking.Services;
using Shared.Redis;
using Shared.Redis.Streaming;
using Shared.Serialization;
using Shared.Streaming;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHttpContextAccessor()
    .AddStreaming()
    .AddSerialization()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming()
    .AddSingleton<MatchmakingRequestsChannel>()
    .AddSingleton<IScoresGenerator,ScoresGenerator>()
    .AddHostedService<MatchmakingBackgroundService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/genscores/start", async (HttpContext context, MatchmakingRequestsChannel channel) =>
{
    await channel.Requests.Writer.WriteAsync(new StartMatchmaking());
    return Results.Ok();
});
app.MapPost("/genscores/stop", async (MatchmakingRequestsChannel channel) =>
{
    await channel.Requests.Writer.WriteAsync(new StopMatchmaking());
    return Results.Ok();
});

app.Run();
