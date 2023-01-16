using GamePlayer.Matchmaking.Models;

namespace GamePlayer.Matchmaking.Services;

internal interface IScoresGenerator
{
    IAsyncEnumerable<MatchResult> StartAsync(int matchId);
    Task StopAsync(int matchId);
    
}