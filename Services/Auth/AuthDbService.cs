using elfak_futmgr.Models;

namespace elfak_futmgr.Services.Auth;

public interface IAuthDbService
{
    public Task RegisterUser(AuthorizedUser authorizedUser);
    public Task<AuthorizedUser?> FetchUser(string username);
}
public class AuthDbService : IAuthDbService
{

    public AuthDbService()
    {
    }

    public async Task RegisterUser(AuthorizedUser authorizedUser)
    {
        
    }

    public async Task<AuthorizedUser?> FetchUser(string username)
    {
        return new AuthorizedUser()
        {
            Username = "Marko",
            Id = 1,
            Role = "Authorized"
        };
    }
}