using Shared.Models;
using Shared.Models.MatchModels;

namespace MatchPlayer.Matchmaking.Requests
{
    internal interface IMatchmakingRequest
    {
        public Match Match { get; set; }
    }
}

