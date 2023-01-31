using Shared.Models.DtoModels;

namespace Shared.Models.MatchModels;

public class Match
{
    public int Id { get; set; }
    public SquadDto HomeSquadDto { get; set; }
    public SquadDto AwaySquadDto { get; set; }
    public DateTime MatchTime { get; set; }
    public string? Result { get; set; }
}