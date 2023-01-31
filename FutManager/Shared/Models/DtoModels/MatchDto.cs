namespace Shared.Models.DtoModels
{
    public class MatchDto
    {
        public int Id { get; set; }
        public string? Result { get; set; }
        public DateTime MatchTime { get; set; }

        public MatchDto(Match m)
        {
            Id = m.Id;
            Result = null;
            MatchTime = m.MatchTime;
        }
    }
}

