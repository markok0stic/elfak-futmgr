using Shared.Models;
using Shared.Models.MatchModels;

namespace MatchPlayer.Matchmaking.Requests
{
    internal record StartMatchmaking : IMatchmakingRequest
    {
        public Match Match { get; set; }
    }
}
