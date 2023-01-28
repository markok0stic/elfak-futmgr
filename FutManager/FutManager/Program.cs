using FutManager.Controllers;
using FutManager.Hubs;
using FutManager.Services;
using Shared.Neo4j;
using Shared.Redis;
using Shared.Redis.Server;
using Shared.Redis.Streaming;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
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
    .AddNeo4J(builder.Configuration)
    .AddRedis(builder.Configuration)
    .AddRedisStreaming()
    .AddRedisServer()
    .AddSingleton<RedisHub>()
    .AddTransient<IHomeService,HomeService>()
    .AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseCors("AllowSpecificOrigin")
    .UseAuthentication()
    .UseAuthorization()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/");
        endpoints.MapHub<RedisHub>("/redisHub");
    });
app.Run();