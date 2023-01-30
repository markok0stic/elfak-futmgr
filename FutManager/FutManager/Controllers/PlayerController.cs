using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Shared.Models.FootballPlayer;
using Shared.Models.Squaq;
using Shared.Neo4j.DbService;

namespace FutManager.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IGraphClient _graphClient;
        private readonly IGraphDbService<Player, Squad?> _graphPlayerDbService;

        public PlayerController(IGraphClient graphClient, IGraphDbService<Player, Squad?> graphPlayerDbService)
        {
            _graphClient = graphClient;
            _graphPlayerDbService = graphPlayerDbService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody]Player player)
        {
            IActionResult response;
            try
            {
                player.Id = await _graphPlayerDbService.GetNextId(typeof(Player).ToString());
                await _graphPlayerDbService.AddNode(player);
                response = Ok();
            }
            catch (Exception e)
            {
                response = UnprocessableEntity(e);
            }

            return response;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlayers(int page)
        {
            IActionResult response;
            try
            {
                await _graphPlayerDbService.GetNodes(page);
                response = Ok();
            }
            catch (Exception e)
            {
                response = UnprocessableEntity(e);
            }

            return response;
        }

    }
}
