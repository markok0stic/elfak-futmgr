namespace GamePlayer.Matchmaking.Requests;

internal interface IMatchmakingRequest
{
    public string Channel { get; set; }
    public int MatchId { get; set; }
}