using System.Collections.Concurrent;
using Shared.Models;

namespace GamePlayer.Matchmaking.Services;

internal sealed class ScoresGenerator: IScoresGenerator
{
    private readonly ConcurrentDictionary<int, bool> _matchRunningStatus;
    private readonly ILogger<IScoresGenerator> _logger;

    public ScoresGenerator(ILogger<ScoresGenerator> logger)
    {
        _logger = logger;
        _matchRunningStatus = new ConcurrentDictionary<int, bool>();
    }

    public async IAsyncEnumerable<MatchResult> StartAsync(int matchId)
    {
        _matchRunningStatus[matchId] = true;
        while (_matchRunningStatus.TryGetValue(matchId, out var isRunning) && isRunning)
        {
            var result = new MatchResult()
            {
                Winner = "Benfica",
                TimeStamp = DateTimeOffset.Now
            };
            _logger.LogInformation("Match winner: {ResultWinner} Time:{ResultTimeStamp}", result.Winner, result.TimeStamp);
            yield return result;
            await Task.Delay(1000);
        }
    }

    public Task StopAsync(int matchId)
    {
        _matchRunningStatus[matchId] = false;
        return Task.CompletedTask;
    }
}