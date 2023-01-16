using Shared.Models;
using Shared.Streaming;

namespace WebApplication1;

internal sealed class Worker : BackgroundService
{
    private readonly IStreamSubscriber _subscriber;
    private readonly ILogger<Worker> _logger;

    public Worker(IStreamSubscriber subscriber,
        ILogger<Worker> logger)
    {
        _subscriber = subscriber;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _subscriber.SubscribeAsync<MatchResult>("scores_1", sub =>
        {
            _logger.LogInformation("Winner is {ObjWinner} and TS: {ObjTimeStamp }", sub.Winner, sub.TimeStamp);
        });
    }
}