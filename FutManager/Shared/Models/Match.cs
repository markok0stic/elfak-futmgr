namespace Shared.Models
{
    public class Match
    {
        public Squad HomeSquad { get; set; }
        public Squad AwaySquad { get; set; }
        public string Result { get; set; }
        public int ID { get; set; }
    }
}

