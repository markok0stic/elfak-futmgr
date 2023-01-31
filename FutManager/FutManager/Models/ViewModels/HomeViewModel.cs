using Shared.Models.DtoModels;
using Shared.Models.MatchModels;

namespace FutManager.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<CurrentlyLiveMatch>? LiveMatches { get; set; }
        public IEnumerable<PlayerDto>? Players { get; set; }
        public IEnumerable<SquadDto>? Squads { get; set; }
    }
}

