using Microsoft.AspNetCore.Mvc;

namespace FutManager.Controllers;

public class HomeController : Controller
{
   public async Task<ViewResult> Index()
   {
      ViewData["Title"] = "Home";
      return View();
   }
}