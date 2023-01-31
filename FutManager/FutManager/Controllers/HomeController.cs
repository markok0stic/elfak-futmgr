using FutManager.Models.ViewModels;
using FutManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace FutManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<ViewResult> Index()
        {
            var model = HomeViewModel.Instance;
            model.LiveMatches = await _homeService.GetAllActiveMatches();
            model.Players = await _homeService.GetPlayersForPage(0);
            model.Squads = await _homeService.GetSquadsForPage(0);

            ViewData["Title"] = "Home";
            return View(model);
        }
    }
}

