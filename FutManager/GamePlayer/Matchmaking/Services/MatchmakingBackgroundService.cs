using GamePlayer.Matchmaking.Requests;
using Newtonsoft.Json;
using Shared.Streaming;
using System.Collections.Concurrent;

namespace GamePlayer.Matchmaking.Services;

internal sealed class MatchmakingBackgroundService : BackgroundService
{
    private readonly IScoresGenerator _scoresGenerator;
    private readonly MatchmakingRequestsChannel _matchmakingRequestsChannel;
    private readonly IStreamPublisher _streamPublisher;
    private readonly ILogger<MatchmakingBackgroundService> _logger;
    private readonly ConcurrentDictionary<int, Task> _matchTasks;
    private int _matchCounter;

    public MatchmakingBackgroundService(IScoresGenerator scoresGenerator,
        MatchmakingRequestsChannel matchmakingRequestsChannel, IStreamPublisher streamPublisher,
        ILogger<MatchmakingBackgroundService> logger)
    {
        _matchCounter = 0;
        _scoresGenerator = scoresGenerator;
        _matchmakingRequestsChannel = matchmakingRequestsChannel;
        _streamPublisher = streamPublisher;
        _logger = logger;
        _matchTasks = new ConcurrentDictionary<int, Task>();
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Matchmaking background service has started.");
        await foreach (var request in _matchmakingRequestsChannel.Requests.Reader.ReadAllAsync(stoppingToken))
        {
            _logger.LogInformation(
                $"Matchmaking background service has received the request: '{request.GetType().Name}'.");

            var _ = request switch
            {
                StartMatchmaking => StartMatchmakingAsync(),
                StopMatchmaking => StopMatchmakingAsync(),
                _ => Task.CompletedTask
            };
        }

        _logger.LogInformation("Matchmaking background service has stopped.");
    }

    private async Task StartMatchmakingAsync()
    {
        int matchId = Interlocked.Increment(ref _matchCounter);
        var matchTask = StartMatchAsync(matchId);
        _matchTasks.TryAdd(matchId, matchTask);
    }

    private async Task StartMatchAsync(int matchId)
    {
        await foreach (var scores in _scoresGenerator.StartAsync(matchId))
        {
            _logger.LogInformation($"Publishing the scores for match {matchId}...");
            await _streamPublisher.PublishAsync($"scores_{matchId}", scores);
        }

        _matchTasks.TryRemove(matchId, out _);
    }

    private async Task StopMatchmakingAsync()
    {
        if (_matchTasks.TryGetValue(_matchCounter, out var matchTask))
        {
            await _scoresGenerator.StopAsync(_matchCounter);
            _matchTasks.TryRemove(_matchCounter, out _);
            Interlocked.Decrement(ref _matchCounter);
        }
        else
        {
            _logger.LogInformation("No match found to stop.");
        }
    }
}
