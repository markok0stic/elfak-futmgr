using Shared.Enums;
using Shared.Models.Football_Player_Models;

namespace Shared.Models.MatchModels
{
    public class MatchLiveMessage: Match
    {
        public string Message { get; set; }
        public Score? Score { get; set; }
        public Card? Card { get; set; }

        public MatchLiveMessage(Match match)
        {
            Id = match.Id;
            HomeSquad = match.HomeSquad;
            AwaySquad = match.AwaySquad;
            Scores = match.Scores;
            Result = match.Result;
            TimeStamp = match.TimeStamp;
            Message = string.Empty;
            Score = null;
            Card = null;
        }
    }
}

