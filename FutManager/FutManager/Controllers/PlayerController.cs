using FutManager.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.FootballPlayer;

namespace FutManager.Controllers
{
    [ApiController]
    [Route("PlayerManager")]
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        
        [Route("AddPlayer")]        
        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody]Player player)
        {
            int response = await _playerService.AddPlayer(player);
            return StatusCode(response);
        }
        
        [Route("GetPlayers/{page}")]
        [HttpGet]
        public async Task<IActionResult> GetPlayers(int page)
        {
            var players = await _playerService.GetPlayersForPage(page);
            return Ok(players);
        }
        
        [Route("DeletePlayer/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            int response = await _playerService.DeletePlayer(id);
            return StatusCode(response);
        }

        [Route("GetPlayer/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPlayer(int id)
        {
            var player = await _playerService.GetPlayer(id);
            return Ok(player);
        }

        [Route("UpdatePlayer")]
        [HttpPut]
        public async Task<IActionResult> UpdatePlayer([FromBody] Player player)
        {
            int response = await _playerService.UpdatePlayer(player);
            return StatusCode(response);
        }

        [Route("AddPlayerToSquad/{SquadId}/{PlayerId}")]
        [HttpPost]
        public async Task<IActionResult> AddPlayerToSquad(int SquadId, int PlayerId)
        {
            int response = await _playerService.AddPlayerToSquad(SquadId, PlayerId);
            return StatusCode(response);
        }
    }
}
