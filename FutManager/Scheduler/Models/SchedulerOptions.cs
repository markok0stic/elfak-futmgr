namespace Scheduler.Models;

public sealed class SchedulerOptions
{
    public string MatchPlayerBaseUrl { get; set; }
    public string MatchPlayerApiKey { get; set; }
    public string AggregatorBaseUrl { get; set; }
    public string AggregatorApiKey { get; set; }
}