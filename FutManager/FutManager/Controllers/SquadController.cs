using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Neo4jClient.Cypher;

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
            var query = _graphClient.Cypher.Create("(n:Squad{name:'" + name + "', balance:'" + balance + "'})");

            await query.ExecuteWithoutResultsAsync();

            return Ok();
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
