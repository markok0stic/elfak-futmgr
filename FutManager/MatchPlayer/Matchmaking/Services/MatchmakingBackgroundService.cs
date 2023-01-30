using Shared.Streaming;
using System.Collections.Concurrent;
using MatchPlayer.Matchmaking.Requests;
using Shared.Models;
using Shared.Models.MatchModels;

namespace MatchPlayer.Matchmaking.Services
{
    internal sealed class MatchmakingBackgroundService : BackgroundService
    {
        private readonly ILiveMessageGenerator _liveMessageGenerator;
        private readonly MatchmakingRequestsChannel _matchmakingRequestsChannel;
        private readonly IStreamPublisher _streamPublisher;
        private readonly ILogger<MatchmakingBackgroundService> _logger;
        private readonly ConcurrentDictionary<int, Task> _matchTasks;
        private int _matchCounter;

        public MatchmakingBackgroundService(ILiveMessageGenerator liveMessageGenerator,
            MatchmakingRequestsChannel matchmakingRequestsChannel, IStreamPublisher streamPublisher,
            ILogger<MatchmakingBackgroundService> logger)
        {
            _matchCounter = 0;
            _liveMessageGenerator = liveMessageGenerator;
            _matchmakingRequestsChannel = matchmakingRequestsChannel;
            _streamPublisher = streamPublisher;
            _logger = logger;
            _matchTasks = new ConcurrentDictionary<int, Task>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Matchmaking background service has started.\n Waiting for scheduler to start match.");
            await foreach (var request in _matchmakingRequestsChannel.Requests.Reader.ReadAllAsync(stoppingToken))
            {
                _logger.LogInformation($"Matchmaking background service has received the request: '{request.GetType().Name}' with matchId: '{request.Match.Id}'.");

                var _ = request switch
                {
                    StartMatchmaking => StartMatchmakingAsync(request.Match),
                    StopMatchmaking => StopMatchmakingAsync(request.Match.Id),
                    _ => Task.CompletedTask
                };
            }

            _logger.LogInformation("Matchmaking background service has stopped.");
        }

        private async Task StartMatchmakingAsync(Match match)
        {
            Interlocked.Increment(ref _matchCounter);
            var matchTask = StartMatchAsync(match);
            _matchTasks.TryAdd(match.Id, matchTask);
        }

        private async Task StartMatchAsync(Match match)
        {
            await foreach (var scores in _liveMessageGenerator.StartAsync(match))
            {
                _logger.LogInformation($"Publishing the scores for match {match.Id}...");
                await _streamPublisher.PublishAsync($"match_{match.Id}", scores);
            }
            _matchTasks.TryRemove(match.Id, out _);
        }

        private async Task StopMatchmakingAsync(int matchId)
        {
            if (_matchTasks.TryGetValue(matchId, out _))
            {
                await _liveMessageGenerator.StopAsync(matchId);
                _matchTasks.TryRemove(matchId, out _);
                Interlocked.Decrement(ref _matchCounter);
            }
            else
            {
                _logger.LogInformation("No match found to stop.");
            }
        }
    }
}

