using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using elfak_futmgr.Models;
using Microsoft.IdentityModel.Tokens;

namespace elfak_futmgr.Helpers;

public static class JwtTokenHelper
{
    public static string CreateToken(AuthorizedUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid,user.Id.ToString()),
            new Claim(ClaimTypes.Role,user.Role),
            new Claim(ClaimTypes.Name,user.Username),
            new Claim(ClaimTypes.Expiration,(DateTime.Now.AddSeconds(1)).ToString("yyyy-MM-dd HH:mm:ss"))
        };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("SecretTokenForMyApp"));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires:DateTime.Now.AddHours(1),
            signingCredentials:cred);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static AuthorizedUser? ReadTokenClaims(HttpContext? httpContext)
    {
        var user = new AuthorizedUser();
        if (httpContext != null)
        {
            var id  = httpContext.User.FindFirstValue(ClaimTypes.Sid);
            var role = httpContext.User.FindFirstValue(ClaimTypes.Role);
            var username = httpContext.User.FindFirstValue(ClaimTypes.Name);
            if (id == null || role == null || username == null)
                user = null;
            else
            {
                user.Id = Int32.Parse(id);
                user.Role = role;
                user.Username = username;
            }
        }
        else
        {
            user = null;
        }
        return user;
    }
    
    public static int ReadTokenUserId(HttpContext? httpContext)
    {
        int id = -1;
        if (httpContext != null)
        {
            var userId  = httpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (userId != null)
                id = Convert.ToInt32(userId);
        }
        return id;
    }

    public static AuthorizedUser? DecodeToken(string? token)
    {
        var user = new AuthorizedUser();
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        if (jwtSecurityToken.Claims.Any(claim => claim.Type == ClaimTypes.Sid) &&
            jwtSecurityToken.Claims.Any(claim => claim.Type == ClaimTypes.Role) &&
            jwtSecurityToken.Claims.Any(claim => claim.Type == ClaimTypes.Name))
        {
            user.Id = Convert.ToInt32(jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value);
            user.Username = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            user.Role = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
        }
        else
        {
            user = null;
        }
        return user;
    }

    public static bool IsAuthenticated(HttpContext context)
    {
        bool result = false;
        if (context.User.Identity != null)
        {
            result = context.User.Identity.IsAuthenticated;
        }

        return result;
    }
    
    public static string? GetRoleIfAuthorized(HttpContext context)
    {
        string? result = null;
        if (context.User.Identity != null)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                result = context.User.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            }
        }
        return result;
    }
}