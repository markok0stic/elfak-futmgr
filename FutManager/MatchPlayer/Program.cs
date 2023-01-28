using MatchPlayer.Matchmaking.Requests;
using MatchPlayer.Matchmaking.Services;
using Newtonsoft.Json;
using Shared.Redis;
using Shared.Redis.Streaming;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => 
            policyBuilder
                .WithOrigins("https://localhost:7044")
                .AllowAnyHeader()
                .AllowAnyMethod());
});
builder.Services
    .AddHttpContextAccessor()
    .AddRedis(builder.Configuration)
    .AddRedisStreaming()
    .AddSingleton<MatchmakingRequestsChannel>()
    .AddSingleton<ILiveMessageGenerator, LiveMessageGenerator>()
    .AddHostedService<MatchmakingBackgroundService>();

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");
app.MapGet("/", () => "Hello World!");
app.MapPost("/startMatch", async (HttpContext context, MatchmakingRequestsChannel channel) =>
{
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    var data = JsonConvert.DeserializeObject<StartMatchmaking>(requestBody);
    if (data?.Match != null) await channel.Requests.Writer.WriteAsync(data);
    return Results.Ok();
});
app.MapPost("/stopMatch", async (HttpContext context, MatchmakingRequestsChannel channel) =>
{
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    var data = JsonConvert.DeserializeObject<StopMatchmaking>(requestBody);
    if (data != null) await channel.Requests.Writer.WriteAsync(data);
    return Results.Ok();
});
app.Run();
