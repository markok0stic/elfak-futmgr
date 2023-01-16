using GamePlayer.Matchmaking.Requests;
using GamePlayer.Matchmaking.Services;
using Newtonsoft.Json;
using Shared.Redis;
using Shared.Redis.Streaming;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddHttpContextAccessor()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming()
    .AddSingleton<MatchmakingRequestsChannel>()
    .AddSingleton<IScoresGenerator,ScoresGenerator>()
    .AddHostedService<MatchmakingBackgroundService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/genscores/start", async (HttpContext context, MatchmakingRequestsChannel channel) =>
{
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    var data = JsonConvert.DeserializeObject<StartMatchmaking>(requestBody);
    if (data != null) await channel.Requests.Writer.WriteAsync(data);
    return Results.Ok();
});
app.MapPost("/genscores/stop", async (HttpContext context, MatchmakingRequestsChannel channel) =>
{
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    var data = JsonConvert.DeserializeObject<StopMatchmaking>(requestBody);
    if (data != null) await channel.Requests.Writer.WriteAsync(data);
    return Results.Ok();
});

app.Run();
