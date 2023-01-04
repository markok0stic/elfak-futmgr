using System.Security.Claims;
using elfak_futmgr.Filters;
using elfak_futmgr.FIlters;
using elfak_futmgr.Models;
using elfak_futmgr.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace elfak_futmgr.Controllers;

[ViewFilter]
public class AuthController : Controller
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<ViewResult> Index()
    {
        ViewData["Title"] = "Log-In";
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> LogIn([FromBody]UserDto userDto)
    {
        IActionResult result;
        try
        {
            var token = await _authService.AuthenticateUser(userDto, HttpContext.Session);
            if (token != null)
            {
                result = Ok();
            }
            else
            {
                result = BadRequest();
            }
        }
        catch (Exception e)
        {
            result = UnprocessableEntity(e.Message);
        }

        return result;
    }
    
    [CustomAuth(ClaimTypes.Role,"Authorized,SuperAdmin")]
    public async Task<IActionResult> LogOut()
    {
        IActionResult result;
        try
        {
            var status= await _authService.LogoutUser(HttpContext.Session);
            if (status)
            {
                result = RedirectToAction("Index","Auth");
            }
            else
            {
                result = BadRequest();
            }
        }
        catch (Exception e)
        {
            result = UnprocessableEntity(e.Message);
        }

        await Task.CompletedTask;
        return result;
    }
}