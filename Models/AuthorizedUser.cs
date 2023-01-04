using elfak_futmgr.Enums;

namespace elfak_futmgr.Models;
public class AuthorizedUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public string Role { get; set; }

    public AuthorizedUser()
    {
        Id = 0;
        Username =  string.Empty;
        PasswordHash = PasswordSalt = null;
        Role = string.Empty;

    }
    public AuthorizedUser(IConfiguration configuration)
    {
        Id = 0;
        Username =  string.Empty;
        PasswordHash = PasswordSalt = null;
        Role = configuration.GetValue<string[]>("Roles")[(int)AuthRole.Unauthorized];
    }
}