namespace GamePlayer.Matchmaking.Requests;

internal record StopMatchmaking : IMatchmakingRequest
{
    public string Channel { get; set; }
    public int MatchId { get; set; }
}