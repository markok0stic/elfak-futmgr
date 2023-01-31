using Aggregator.Listener.Requests;
using Newtonsoft.Json;
using Shared.Models.DtoModels;
using Shared.Models.FootballPlayer;
using Shared.Models.MatchModels;
using Shared.Neo4j.DbService;
using Shared.Neo4j.Enums;
using Shared.Streaming;

namespace Aggregator.Listener.Services
{
    internal sealed class AggregatorWorker : BackgroundService
    {
        private readonly IStreamSubscriber _subscriber;
        private readonly AggregatorRequestChannel _aggregatorRequest;
        private readonly IGraphDbService<Player,MatchDto> _graphPlayerDbService;
        private readonly IGraphDbService<MatchDto,object?> _graphMatchDbService;
        private readonly ILogger<AggregatorWorker> _logger;

        public AggregatorWorker(IStreamSubscriber subscriber, AggregatorRequestChannel aggregatorRequest, IGraphDbService<Player, MatchDto> graphPlayerDbService, ILogger<AggregatorWorker> logger, IGraphDbService<MatchDto, object?> graphMatchDbService)
        {
            _subscriber = subscriber;
            _aggregatorRequest = aggregatorRequest;
            _graphPlayerDbService = graphPlayerDbService;
            _logger = logger;
            _graphMatchDbService = graphMatchDbService;
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
                    await _graphMatchDbService.UpdateNode(new MatchDto(sub));
                    // stop "listening" to match
                    await StopAggregationAsync(requestMatchId);
                }

                if (sub.Score != null)
                {
                    // persist score
                    await _graphPlayerDbService.MakeRelationship(sub.Score,new MatchDto() {Id = sub.Id}, RelationshipTypes.SCORED_GOAL);
                }
                
                // we can implement here persists of cards etc...
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

