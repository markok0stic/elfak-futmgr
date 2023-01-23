using Shared.Models.Football_Player_Models;
using Shared.Models.Squad_Models;

namespace SquadManager.Builder
{
    internal record SquadBuilder
    {
        public Squad Team { get; set; }
        public List<Player>? Players { get; set; }
    }
}
