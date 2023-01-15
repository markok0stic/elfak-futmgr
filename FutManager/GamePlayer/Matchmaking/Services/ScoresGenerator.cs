using GamePlayer.Matchmaking.Models;
using Shared.Models;

namespace GamePlayer.Matchmaking.Services;

internal sealed class ScoresGenerator: IScoresGenerator
{
    private readonly ILogger<ScoresGenerator> _logger;
    
    private bool _isRunning;
    private readonly Random _random = new();
    private readonly List<MatchResult> _matchResults = new();

    public ScoresGenerator(ILogger<ScoresGenerator> logger)
    {
        _logger = logger;
    }
    
    public async IAsyncEnumerable<MatchResult> StartAsync()
    {
        _isRunning = true;
        while (_isRunning)
        {
            if (!_isRunning)
            {
                yield break;
            }

            var result =  new MatchResult()
            {
                Winner = "Benfica",
                TimeStamp = DateTimeOffset.Now
            };
            _logger.LogInformation("Match winner: {ResultWinner} Time:{ResultTimeStamp}", result.Winner,
                result.TimeStamp);
            yield return result;
            await Task.Delay(1000);
        }
    }

    public Task StopAsync()
    {
        _isRunning = false;
        return Task.CompletedTask;
    }
}