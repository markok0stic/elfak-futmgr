using Shared.Models;
using Shared.Models.FootballPlayer;
using Shared.Models.MatchModels;
using Shared.Models.Squaq;

namespace FutManager.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<CurrentlyLiveMatch> LiveMatches { get; set; }
        public List<Player> Players { get; set; }
        public List<Squad> Squads { get; set; }
    }
}

