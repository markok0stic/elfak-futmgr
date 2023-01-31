using FutManager.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.FootballPlayer;

namespace FutManager.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }
        
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
    }
}
