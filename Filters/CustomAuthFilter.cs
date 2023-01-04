using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace elfak_futmgr.FIlters;

public class CustomAuthAttribute : TypeFilterAttribute 
{ 
        public CustomAuthAttribute(string claimType, string claimValue) : base(typeof(CustomAuthFilter))
        {
                Arguments = new object[1] { new List<Claim>()};
                var claimValues = claimValue.Split(',');
                var claims = new List<Claim>();
                foreach (var value in claimValues)
                {
                        claims.Add(new Claim(claimType,value));
                }
                Arguments[0] = claims;
        }
}
public class CustomAuthFilter : IAuthorizationFilter
{
        readonly List<Claim> _claims;

        public CustomAuthFilter(List<Claim> claims)
        {
                _claims = claims;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
                var isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
                var claimsIndentity = context.HttpContext.User.Identity as ClaimsIdentity;

                if (isAuthenticated)
                {
                        bool flagClaim = false;
                        foreach (var item in _claims)
                        {
                                if (context.HttpContext.User.HasClaim(item.Type, item.Value))
                                        return;
                        }

                        if (!flagClaim)
                        {
                                string red = context.HttpContext.Request.Path.Value +
                                             context.HttpContext.Request.QueryString.Value;
                                context.Result = new RedirectToRouteResult(
                                        new RouteValueDictionary(new { controller = "Home", action = "Index", red })
                                );
                        }
                }
                else
                {
                        string red = context.HttpContext.Request.Path.Value +
                                     context.HttpContext.Request.QueryString.Value;
                                context.Result = new RedirectToRouteResult(
                                        new RouteValueDictionary(new { controller = "Auth", action = "Index", red })
                                );
                }
        }
}