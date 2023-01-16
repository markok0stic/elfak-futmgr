namespace Shared.Models;

public sealed class MatchResult : Match
{
    public List<Score> Scores { get; set; }
    public FinalScore FinalScore { get; set; }
    public string Winner { get; set; }
    public DateTimeOffset TimeStamp { get; set; }
}