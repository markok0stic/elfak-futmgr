using Shared.Enums;

namespace Shared.Models.DtoModels
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? OverallRating { get; set; }
        public int? Age { get; set; }
        public string? Nationality { get; set; }
        public SquadDto? IrlSquad { get; set; }
        public FieldPositions? Position { get; set; }
    }
}

