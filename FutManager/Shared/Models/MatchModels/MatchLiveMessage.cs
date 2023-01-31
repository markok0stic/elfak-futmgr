using Shared.Models.DtoModels;

namespace Shared.Models.MatchModels
{
    public class MatchLiveMessage: Match
    {
        public string Message { get; set; }
        public PlayerDto? Score { get; set; }
        public int Minute { get; set; }
    }
}

