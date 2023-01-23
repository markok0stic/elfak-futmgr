using Shared.Models.Football_Player_Models;
using Shared.Models.Squad_Models;

namespace Shared.Models.MatchModels
{
    public class Match
    {
        public int Id { get; set; }
        public Squad HomeSquad { get; set; }
        public Squad AwaySquad { get; set; }
        public List<Score> Scores { get; set; }
        public string Result { get; set; }
    }
}

