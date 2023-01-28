using Scheduler.Scheduler;
using Scheduler.Scheduler.Services;
using Shared.RestApiClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => 
            policyBuilder
                .WithOrigins("https://localhost:7042")
                .AllowAnyHeader()
                .AllowAnyMethod());
});
builder.Services
    .AddRestApiClient()
    .AddSchedulerOptions(builder.Configuration)
    .AddSingleton<ISchedulerService, SchedulerService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Schedule}/{action=Index}/");
});
app.Run();