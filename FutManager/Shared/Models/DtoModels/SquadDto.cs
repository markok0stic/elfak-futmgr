namespace Shared.Models.DtoModels
{
    public class SquadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Balance { get; set; }
        
        public List<PlayerDto>? Players { get; set; }
    }
}

