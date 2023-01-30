using Shared.Models.FootballPlayer;
using Shared.Models.Squaq;

namespace SquadManager.Builder
{
    internal record SquadBuilder
    {
        public Squad Team { get; set; }
        public List<Player>? Players { get; set; }
    }
}
