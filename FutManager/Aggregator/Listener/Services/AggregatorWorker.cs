using Aggregator.Listener.Requests;
using Newtonsoft.Json;
using Shared.Models;
using Shared.Models.MatchModels;
using Shared.Neo4j.DbService;
using Shared.Streaming;

namespace Aggregator.Listener.Services
{
    internal sealed class AggregatorWorker : BackgroundService
    {
        private readonly IStreamSubscriber _subscriber;
        private readonly AggregatorRequestChannel _aggregatorRequest;
        private readonly IGraphDbService _graphDbService;
        private readonly ILogger<AggregatorWorker> _logger;

        public AggregatorWorker(IStreamSubscriber subscriber, AggregatorRequestChannel aggregatorRequest, IGraphDbService graphDbService, ILogger<AggregatorWorker> logger)
        {
            _subscriber = subscriber;
            _aggregatorRequest = aggregatorRequest;
            _graphDbService = graphDbService;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Aggregator background service has started...");
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
            return _subscriber.SubscribeAsync<MatchLiveMessage>($"match_{requestMatchId}", async sub =>
            {
                // this can be handler from other custom service, but i am going to create an inline implementation of my random handler
                // what i want to succeed just to persist scores, final result... if any happens in the match
                if (sub.Result != null)
                {
                    // persist result
                    
                    // stop "listening" to match
                    await StopAggregationAsync(requestMatchId);
                }

                if (sub.Score != null)
                {
                    // persist scores
                    
                }
                
                _logger.LogInformation(JsonConvert.SerializeObject(sub));
                
            });
        }
        
        private Task StopAggregationAsync(int requestMatchId)
        {
            _logger.LogInformation("Match ended");
            return _subscriber.UnsubscribeAsync($"match_{requestMatchId}");
        }
    }
}

