using Shared.Models;

namespace MatchPlayer.Matchmaking.Services;

internal interface IScoresGenerator
{
    IAsyncEnumerable<MatchResult> StartAsync(int matchId);
    Task StopAsync(int matchId);
    
}