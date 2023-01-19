using System.Collections.Concurrent;
using Shared.Models;

namespace MatchPlayer.Matchmaking.Services;
internal interface IScoresGenerator
{
    IAsyncEnumerable<MatchResult> StartAsync(int matchId);
    Task StopAsync(int matchId);
}
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
        var i = 0;
        while (_matchRunningStatus.TryGetValue(matchId, out var isRunning) && isRunning && i<=30)
        {            
            var result = new MatchResult()
            {
                Winner = "Benfica",
                TimeStamp = DateTimeOffset.Now
            };
            
            if (i == 30)
            {
                result.Winner = $"match_{matchId}";
            }
            
            i++;
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