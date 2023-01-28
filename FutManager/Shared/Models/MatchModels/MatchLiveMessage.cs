using Shared.Enums;
using Shared.Models.Football_Player_Models;

namespace Shared.Models.MatchModels
{
    public class MatchLiveMessage: Match
    {
        public string Message { get; set; }
        public Score? Score { get; set; }
        public Card? Card { get; set; }
        public int Minute { get; set; }
    }
}

