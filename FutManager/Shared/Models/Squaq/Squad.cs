using Shared.Models.FootballPlayer;

namespace Shared.Models.Squaq
{
    public class Squad
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Balance { get; set; }
        public List<Player>? Players { get; set; }
    }
}

