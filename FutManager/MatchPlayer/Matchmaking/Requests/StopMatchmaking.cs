namespace MatchPlayer.Matchmaking.Requests
{
    internal record StopMatchmaking : IMatchmakingRequest
    {
        public int MatchId { get; set; }
    }
}

