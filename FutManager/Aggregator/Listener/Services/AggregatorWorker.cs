using Aggregator.Listener.Requests;
using Shared.Models;
using Shared.Models.MatchModels;
using Shared.Streaming;

namespace Aggregator.Listener.Services
{
    internal sealed class AggregatorWorker : BackgroundService
    {
        private readonly IStreamSubscriber _subscriber;
        private readonly AggregatorRequestChannel _aggregatorRequest;
        private readonly ILogger<AggregatorWorker> _logger;

        public AggregatorWorker(IStreamSubscriber subscriber, AggregatorRequestChannel aggregatorRequest, ILogger<AggregatorWorker> logger)
        {
            _subscriber = subscriber;
            _aggregatorRequest = aggregatorRequest;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Aggregator background service has started.\n Waiting for scheduler to start match.");
            await foreach (var request in _aggregatorRequest.Requests.Reader.ReadAllAsync(stoppingToken))
            {
                var _ = request switch
                {
                    StartAggregation => StartAggregationAsync(request.MatchId),
                    _ => Task.CompletedTask
                };
            }
            _logger.LogInformation("Aggregator background service has stopped.");
        }

        private Task StartAggregationAsync(int requestMatchId)
        {
            return _subscriber.SubscribeAsync<MatchLiveMessage>($"match_{requestMatchId}", sub =>
            {
                /*if (sub.Winner == $"match_{requestMatchId}")
                {
                    _subscriber.UnsubscribeAsync($"match_{requestMatchId}");
                }*/
                _logger.LogInformation( sub.Message);
            });
        }
    }
}

