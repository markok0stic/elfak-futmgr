using Shared.Enums;
using Shared.Models.Squaq;

namespace Shared.Models.FootballPlayer
{
    public class Player
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int OverallRating { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
        public Squad? IrlSquad { get; set; }
        public FieldPositions Position { get; set; }
        public int ID { get; set; }
    }
}

