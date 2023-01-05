using elfak_futmgr.Models;
using Neo4j.Driver;

namespace elfak_futmgr.Services.Auth;

public interface IAuthDbService
{
    public Task RegisterUser(AuthorizedUser authorizedUser);
    public Task<AuthorizedUser?> FetchUser(string username);
}
public class AuthDbService : IAuthDbService
{
    private readonly IDriver _driver;
    
    public AuthDbService(IConfiguration configuration)
    {
        _driver = GraphDatabase.Driver(
            configuration.GetValue<string>("NeoDb:Uri"),
            AuthTokens.Basic(
                configuration.GetValue<string>("NeoDb:Username"),
                configuration.GetValue<string>("NeoDb:Password")
                )
            );
    }
    
    public async Task RegisterUser(AuthorizedUser authorizedUser)
    {
        using var session = _driver.AsyncSession();
        var greeting = await session.ExecuteWriteAsync(async tx =>
        {
            var result = await tx.RunAsync("MERGE (a:User) " +
                                           "SET a.Username = $authorizedUser.Username " +
                                           "SET a.PasswordSalt = $passSalt " +
                                           "SET a.PasswordHash = $passHash " +
                                           "SET a.Role = $authorizedUser.Role " +
                                           "RETURN a",
                new { authorizedUser, 
                    passSalt = System.Text.Encoding.UTF8.GetString(authorizedUser.PasswordSalt), 
                    passHash = System.Text.Encoding.UTF8.GetString(authorizedUser.PasswordHash) 
                });
            return result;
        });
        var a = await greeting.ConsumeAsync();
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