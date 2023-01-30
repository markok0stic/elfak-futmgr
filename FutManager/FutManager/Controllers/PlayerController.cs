using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
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
            var query1 = _graphClient.Cypher.Match("(n:Player)").Return<int>("max(n.ID)");
            int maxId = 0;
            try
            {
                using (var result = query1.ResultsAsync)
                {
                    await result;
                    maxId = Convert.ToInt32(result.Result.FirstOrDefault());
                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }

            var query = _graphClient.Cypher.Create("(n:Player{FirstName:'" + player.FirstName
                                                            + "', LastName:'" + player.LastName
                                                            + "', OverallRating:" + player.OverallRating
                                                            + ", Id:" + (maxId + 1)
                                                            + ", Age:" + player.Age
                                                            + ", Nationality:'" + player.Nationality
                                                            + "', Position:'" + player.Position + "'})");
            await query.ExecuteWithoutResultsAsync();

            return Ok();
        }

        [Route("GetPlayers/{page}")]
        [HttpGet]
        public async Task<IActionResult> GetPlayers(int page)
        {
            int skip = 5 * page;            

            var query = _graphClient.Cypher.Match("(n:Player)")
                                         .Return<Player>("n")
                                         .Skip(skip)
                                         .Limit(5);
           
            var result = await query.ResultsAsync;
            List<Player> players = result.ToList();

            /*return View("Views\\Home\\Index.cshtml", players);*/
            return Ok(result);
        }

    }
}
