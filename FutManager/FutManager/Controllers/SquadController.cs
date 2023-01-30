using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Neo4jClient.Cypher;
using Shared.Models.FootballPlayer;
using Shared.Models.Squaq;

namespace FutManager.Controllers
{
    [ApiController]
    [Route("SquadManager")]
    public class SquadController : Controller
    {
        private readonly IGraphClient _graphClient;

        public SquadController(IGraphClient graphClient)
        {
            _graphClient = graphClient;
        }

        [Route("AddSquad/{name}/{balance}")]
        [HttpPost]
        public async Task<IActionResult> AddSquad(string name, int balance)
        {
            var query1 = _graphClient.Cypher.Match("(n:Player)").Return<int>("max(n.id)");
            int maxId = 0;
            try
            {
                using (var result = query1.ResultsAsync)
                {
                    await result;
                    if (result != null)
                        maxId = Convert.ToInt32(result.Result.FirstOrDefault());
                }
            }
            catch (Exception e)
            {
                string msg = e.Message;
            }
            var query = _graphClient.Cypher.Create("(n:Squad{Name:'" + name 
                                                    + "', Balance:'" + balance 
                                                    + "', Id: " + (maxId+1)+"})");

            await query.ExecuteWithoutResultsAsync();

            return Ok();
        }

        [Route("GetSquads/{page}")]
        [HttpGet]
        public async Task<IActionResult> GetSquads(int page)
        {
            int skip = 5 * page;

            var query = _graphClient.Cypher.Match("(n:Squad)")
                                         .Return<Squad>("n")
                                         .Skip(skip)
                                         .Limit(5); ;
            
            var result = await query.ResultsAsync;
            List<Squad> squads = result.ToList();

            return Ok(result);
        }
        //[Route("ChangeName/{name}")]
        //[HttpPost]
        //public async Task<IActionResult> ChangeName(string name)
        //{
        //    var query = _graphClient.Cypher.Create("(n:Squad{name:'" + name + "', balance:'" + balance + "'})");

        //    await query.ExecuteWithoutResultsAsync();

        //    return Ok();
        //}

        //[Route("ChangeBalance/{balance}")]
        //[HttpPost]
        //public async Task<IActionResult> ChangeBalance(int balance)
        //{
        //    var query = _graphClient.Cypher.Create("(n:Squad{name:'" + name + "', balance:'" + balance + "'})");

        //    await query.ExecuteWithoutResultsAsync();

        //    return Ok();
        //}


    }
}
