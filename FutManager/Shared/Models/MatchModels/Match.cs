namespace Shared.Models.MatchModels
{
    public class Match
    {
        public int Id { get; set; }
        public int HomeSquadId { get; set; }
        public int AwaySquadId { get; set; }
        public string? Result { get; set; }
        public DateTime MatchTime { get; set; }
    }
}

