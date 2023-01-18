using FutManager.Models.ViewModels;
using FutManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace FutManager.Controllers;

public class HomeController : Controller
{
   private readonly IHomeService _homeService;
   
   public HomeController(IHomeService homeService)
   {
      _homeService = homeService;
   }

   public async Task<ViewResult> Index()
   {
      var model = new HomeViewModel();
      model.LiveMatches = await _homeService.GetAllActiveMatches();
      
      ViewData["Title"] = "Home";
      return View(model);
   }
}