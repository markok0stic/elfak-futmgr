using Shared.Enums;
using Shared.Models.Football_Player_Models;

namespace Shared.Models.MatchModels
{
    public record MatchLiveMessage
    {
        public int MatchId { get; set; }
        public string Message { get; set; } = string.Empty;
        public Score? Score { get; set; } = null;
        public Card? Card { get; set; } = null;
    }
}

