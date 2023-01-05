using elfak_futmgr.Enums;
using elfak_futmgr.Helpers;
using elfak_futmgr.Models;

namespace elfak_futmgr.Services.Auth;

public interface IAuthService
{
    public Task<AuthorizedUser> Register(UserDto userDto, bool superAdmin = false);
    public Task<string?> AuthenticateUser(UserDto userDto, ISession session);
    public Task<bool> LogoutUser(ISession session);
}

public class AuthService : IAuthService
{
    private readonly IAuthDbService _authDbService;
    private readonly IConfiguration _configuration;

    public AuthService(IAuthDbService authDbService,IConfiguration configuration)
    {
        _authDbService = authDbService;
        _configuration = configuration;
    }
    
    public async Task<AuthorizedUser> Register(UserDto userDto, bool superAdmin)
    {
        PasswordHelper.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var authUser = new AuthorizedUser
        {
            Username = userDto.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = _configuration.GetSection("Roles").Get<string[]>()[(int)AuthRole.Authorized]
        };
        if (superAdmin)
        {
            authUser.Role = _configuration.GetSection("Roles").Get<string[]>()[(int)AuthRole.SuperAdmin];
        }
        await _authDbService.RegisterUser(authUser);
        return authUser;
    }
    public async Task<string?> AuthenticateUser(UserDto userDto, ISession session)
    {
        var authorizedUser = await _authDbService.FetchUser(userDto.Username);
        if (authorizedUser != null)
        {
            PasswordHelper.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
            if (authorizedUser.PasswordSalt != null
                && authorizedUser.PasswordHash != null
                && PasswordHelper.VerifyPasswordHash(userDto.Password, authorizedUser.PasswordHash,
                    authorizedUser.PasswordSalt))
            {
                
                var token = JwtTokenHelper.CreateToken(authorizedUser);
                session.SetString("JWToken", token);
                return token;
            }
        }
        return null;
    }
    public async Task<bool> LogoutUser(ISession session)
    {
        var status = true;
        session.Clear();
        await Task.CompletedTask;
        return status;
    }
}