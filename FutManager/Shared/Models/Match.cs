using Shared.Models.SquadModels;

namespace Shared.Models;

public class Match
{
    public int Id { get; set; }
    public Squad HomeSquad { get; set; }
    public Squad AwaySquad { get; set; }
    public DateTime MatchTime { get; set; }
}