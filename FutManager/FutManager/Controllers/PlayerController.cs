using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Shared.Models.Football_Player_Models;
using Shared.Models.FootballPlayer;

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
            var query1 = _graphClient.Cypher.Match("(n:Player)").Return<string>("max(n.id)");
            int maxId = 0;
            try
            {
                using (var result = query1.ResultsAsync)
                {
                    await result;
                    if (result != null)
                        maxId = Convert.ToInt32(result);
                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }

            var query = _graphClient.Cypher.Create("(n:Player{firstname:'" + player.FirstName
                                                            + "', lastname:'" + player.LastName
                                                            + "', rating:" + player.OverallRating
                                                            + ", id:" + maxId + 1
                                                            + ", age:'" + player.Age
                                                            + "', nationality:'" + player.Nationality
                                                            + "', position:'" + player.Position + "'})");
            await query.ExecuteWithoutResultsAsync();

            return Ok();
        }

        [Route("GetPlayers/{page}")]
        [HttpGet]
        public async Task<IActionResult> GetPlayers(int page)
        {
            int start = 5 * page;
            int end = start + 5;

            var query = _graphClient.Cypher.Match("(n:Player)")
                                         .Where("(n.id>" + start + " and n.id<" + end + ")")
                                         .Return<Player>("n");
            var result = await query.ExecuteWithoutResultsAsync();
        }

    }
}
