using Neo4jClient;
using Shared.Neo4jClient;
using Shared.Redis;
using Shared.Redis.Streaming;
using Shared.Serialization;
using Shared.Streaming;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services
    .AddHttpContextAccessor()
    .AddSerialization()
    .AddStreaming()
    .AddNeo4J(builder.Configuration)
    .AddRedis(builder.Configuration)
    .AddRedisStreaming();

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

app.UseHttpsRedirection();
/* might be necessary 
app.UseStaticFiles();
*/
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/");
});
app.MapControllers();
app.Run();