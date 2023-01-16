using Shared.Models;

namespace GamePlayer.Matchmaking.Models;

public sealed class MatchResult : Match
{
    internal List<Score> Scores { get; set; }
    internal FinalScore FinalScore { get; set; }
    internal string Winner { get; set; }
    internal DateTimeOffset TimeStamp { get; set; }
}