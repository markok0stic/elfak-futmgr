using elfak_futmgr.Models;
using Neo4j.Driver;
using Neo4jClient;

namespace elfak_futmgr.Services.Auth;

public interface IAuthDbService
{
    public Task RegisterUser(AuthorizedUser authorizedUser);
    public Task<AuthorizedUser?> FetchUser(string username);
}
public class AuthDbService : IAuthDbService
{
    private readonly IGraphClient _client;
    
    public AuthDbService(IGraphClient client)
    {
        _client = client;
    }
    
    public async Task RegisterUser(AuthorizedUser authorizedUser)
    {
        // registration
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