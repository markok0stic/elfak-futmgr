namespace Shared.Models;

public class Player
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int OverallRating { get; set; }
    public int Age { get; set; }
    public string Nationality { get; set; }
    public Squad IrlSquad { get; set; }
}