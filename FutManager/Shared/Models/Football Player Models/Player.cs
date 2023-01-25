using Shared.Enums;
using Shared.Models.Squad_Models;

namespace Shared.Models.Football_Player_Models
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

