using Shared.Enums;
using Shared.Models.DtoModels;
using Shared.Models.FootballPlayer;

namespace Shared.Models.MatchModels
{
    public class MatchLiveMessage: Match
    {
        public string Message { get; set; }
        public Player? Score { get; set; }
        public int Minute { get; set; }
        public string? Result { get; set; }
    }
}

