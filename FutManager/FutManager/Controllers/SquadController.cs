using FutManager.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.DtoModels;

namespace FutManager.Controllers
{
    [ApiController]
    [Route("SquadManager")]
    public class SquadController : Controller
    {
        private readonly ISquadService _squadService;
        public SquadController(ISquadService squadService)
        {
            _squadService = squadService;
        }
    
        [Route("AddSquad/{name}/{balance}")]
        [HttpPost]
        public async Task<IActionResult> AddSquad(string name, int balance)
        {
            int response = await _squadService.AddSquad(name, balance);
            return StatusCode(response);
        }
         
        [Route("GetSquads/{page}")]
        [HttpGet]
        public async Task<IActionResult> GetSquads(int page)
        {
            var squads = await _squadService.GetSquadsForPage(page);
            return Ok(squads);
        }
        
        [Route("DeleteSquad/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteSquad(int id)
        {
            int response = await _squadService.DeleteSquad(id);
            return StatusCode(response);
        }

        [Route("UpdateSquad")]
        [HttpPut]
        public async Task<IActionResult> UpdateSquad([FromBody]SquadDto squadDto)
        {
            int response = await _squadService.UpdateSquad(squadDto);
            return StatusCode(response);
        }
    }
}
