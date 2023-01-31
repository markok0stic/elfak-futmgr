using Newtonsoft.Json;
using Shared.Models.FootballPlayer;

namespace Shared.Models.SquadModels
{
    public class Squad
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Balance { get; set; }
        [JsonIgnore]
        public List<Player>? Players { get; set; }
    }
}

