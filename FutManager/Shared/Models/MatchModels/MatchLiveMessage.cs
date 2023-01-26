using Shared.Enums;
using Shared.Models.Football_Player_Models;

namespace Shared.Models.MatchModels
{
    public class MatchLiveMessage: Match
    {
        public string Message { get; set; } = string.Empty;
        public Score? Score { get; set; } = null;
        public Card? Card { get; set; } = null;
    }
}

