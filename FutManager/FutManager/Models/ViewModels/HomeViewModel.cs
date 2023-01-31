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

        private static readonly object _lock = new object();
        private static HomeViewModel instance = null;
        public static HomeViewModel Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new HomeViewModel();
                    }
                    return instance;
                }
            }
        }
    }
}

