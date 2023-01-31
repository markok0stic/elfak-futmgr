using Shared.Models.FootballPlayer;
using Shared.Models.MatchModels;
using Shared.Models.SquadModels;

namespace FutManager.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<CurrentlyLiveMatch>? LiveMatches { get; set; }
        public IEnumerable<Player>? Players { get; set; }
        public IEnumerable<Squad>? Squads { get; set; }
    }
}

