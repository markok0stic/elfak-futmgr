using Aggregator.Listener.Requests;
using Aggregator.Listener.Services;
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
    .AddSingleton<AggregatorRequestChannel>()
    .AddHostedService<AggregatorWorker>();

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");
app.MapGet("/", () => "Hello World!");
app.MapPost("/aggregateMatch", async (HttpContext context, AggregatorRequestChannel channel) =>
{
    var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
    var data = JsonConvert.DeserializeObject<StartAggregation>(requestBody);
    if (data != null) await channel.Requests.Writer.WriteAsync(data);
    return Results.Ok();
});
app.Run();