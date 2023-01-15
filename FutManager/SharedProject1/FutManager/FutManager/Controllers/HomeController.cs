using Microsoft.AspNetCore.Mvc;

namespace FutManager.Controllers;

/*[CustomAuth(ClaimTypes.Role,"Authorized")]*/
public class HomeController : Controller
{
   
   public async Task<ViewResult> Index()
   {
      ViewData["Title"] = "Log-In";
      return View();
   }
   public string SampleGet()
   {
      return "true";
   }
}