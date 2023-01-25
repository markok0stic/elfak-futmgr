using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Shared.Models.Football_Player_Models;

namespace FutManager.Controllers
{
    [ApiController]
    [Route("PlayerManager")]
    public class PlayerController : Controller
    {
        private readonly IGraphClient _graphClient;

        public PlayerController(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        [Route("AddPlayer")]
        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody] Player player)
        {
            var query = _graphClient.Cypher.Create("(n:Player{firstname:'" + player.FirstName
                                                            + "', lastname:'" + player.LastName
                                                            + "', rating:'" + player.OverallRating
                                                            + "', age:'" + player.Age
                                                            + "', nationality:'" + player.Nationality
                                                            + "', position:'" + player.Position + "'})");
            await query.ExecuteWithoutResultsAsync();

            return Ok();
        }

    }
}
