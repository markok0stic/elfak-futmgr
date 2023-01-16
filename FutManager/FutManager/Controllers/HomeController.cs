using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Neo4jClient;

namespace FutManager.Controllers;

/*[CustomAuth(ClaimTypes.Role,"Authorized")]*/
public class HomeController : Controller
{
   
   public HomeController(IOptions<Neo4JOptions> neo)
   {
      
   }
   
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