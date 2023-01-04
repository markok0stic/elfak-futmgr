using elfak_futmgr.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace elfak_futmgr.Filters;

public class ViewFilter: ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string controller = (string) context.RouteData.Values["controller"];
        string action = (string)context.RouteData.Values["action"];
        if (JwtTokenHelper.IsAuthenticated(context.HttpContext))
        {
            if (controller.ToLower() == "auth" && action.ToLower() != "logout")
            {
                string red = context.HttpContext.Request.Path.Value +
                             context.HttpContext.Request.QueryString.Value;
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Home", action = "Index", red })
                );
            }
        }
    }
}