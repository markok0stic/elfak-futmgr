using FutManager.Models.ViewModels;
using FutManager.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.FootballPlayer;
using Shared.Models.MatchModels;

namespace FutManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly IPlayerService _playerService;
        private readonly ISquadService _squadService;

        public HomeController(IHomeService homeService, IPlayerService playerService, ISquadService squadService)
        {
            _homeService = homeService;
            _playerService = playerService;
            _squadService = squadService;
        }

        public async Task<ViewResult> Index()
        {
            var model = new HomeViewModel();
            model.LiveMatches = await _homeService.GetAllActiveMatches();
            model.Players = await _playerService.GetPlayersForPage(0);
            model.Squads = await _squadService.GetSquadsForPage(0);

            ViewData["Title"] = "Home";
            return View(model);
        }
    }
}

