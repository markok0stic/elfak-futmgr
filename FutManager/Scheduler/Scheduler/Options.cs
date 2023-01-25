using Scheduler.Models;

namespace Scheduler.Scheduler;

public static class Options
{
    public static IServiceCollection AddSchedulerOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<SchedulerOptions>(configuration.GetSection("SchedulerOptions"));
    }
}