using FutManager.Models.ViewModels;
using FutManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Neo4jClient;
using Shared.Models.FootballPlayer;
using Shared.Models.Squaq;
using Shared.Neo4j.DbService;
using System.Numerics;
using System.Xml.Linq;

namespace FutManager.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly IGraphClient _graphClient;
        private readonly IGraphDbService<Player, Squad?> _graphPlayerDbService;

        public PlayerController(IGraphClient graphClient, IGraphDbService<Player, Squad?> graphPlayerDbService, IHomeService homeService)
        {
            _homeService = homeService;
            _graphClient = graphClient;
            _graphPlayerDbService = graphPlayerDbService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody]Player player)
        {
            int response = await _homeService.AddPlayer(player);
            return StatusCode(response);
        }
        //MARE
        //{
        //    IActionResult response;
        //    try
        //    {
        //        player.Id = await _graphPlayerDbService.GetNextId(typeof(Player).ToString());
        //        await _graphPlayerDbService.AddNode(player);
        //        response = Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        response = UnprocessableEntity(e);
        //    }

        //    return response;
        //}

        [Route("GetPlayers/{page}")]
        [HttpGet]
        public async Task<IActionResult> GetPlayers(int page)
        {
            var model = HomeViewModel.Instance;
            model.Players = await _homeService.GetPlayersForPage(page);
            return Ok(model.Players);
        }
        //MARE
        //{
        //    IActionResult response;
        //    try
        //    {
        //        await _graphPlayerDbService.GetNodes(page);
        //        response = Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        response = UnprocessableEntity(e);
        //    }

        //    return response;
        //}

        [Route("DeletePlayer/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            int response = await _homeService.DeletePlayer(id);
            return StatusCode(response);
        }
    }
}
