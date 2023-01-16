namespace GamePlayer.Matchmaking.Requests;

internal record StartMatchmaking : IMatchmakingRequest
{
    public string Channel { get; set; }
    public int MatchId { get; set; }
}