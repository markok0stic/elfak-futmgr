namespace MatchPlayer.Matchmaking.Requests
{
    internal record StartMatchmaking : IMatchmakingRequest
    {
        public int MatchId { get; set; }
    }
}
