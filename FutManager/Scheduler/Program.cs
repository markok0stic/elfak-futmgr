using Scheduler.Scheduler;
using Scheduler.Scheduler.Services;
using Shared.RestApiClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services
    .AddRestApiClient()
    .AddSchedulerOptions(builder.Configuration)
    .AddSingleton<ISchedulerService, SchedulerService>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Schedule}/{action=Index}/");
});
app.Run();