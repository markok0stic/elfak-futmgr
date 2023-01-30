using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using Shared.Models.FootballPlayer;
using Shared.Models.Squaq;
using Shared.Neo4j.DbService;

namespace FutManager.Controllers
{
    public class SquadController : Controller
    {
        private readonly IGraphClient _graphClient;
        private readonly IGraphDbService<Squad, Player?> _graphSquadDbService;

        public SquadController(IGraphClient graphClient, IGraphDbService<Squad, Player?> graphSquadDbService)
        {
            _graphClient = graphClient;
            _graphSquadDbService = graphSquadDbService;
        }
        
        [HttpPost]
        public async Task<IActionResult> AddSquad(string name, int balance)
        {
            IActionResult response ;
            try
            {
                var squad = new Squad()
                {
                    Id = await _graphSquadDbService.GetNextId(typeof(Squad).ToString()),
                    Balance = balance,
                    Name = name
                };
                await _graphSquadDbService.AddNode(squad);
                response = Ok();
            }
            catch (Exception e)
            {
                response = UnprocessableEntity(e);
            }

            return response;
        }

        [HttpGet]
        public async Task<IActionResult> GetSquads(int page)
        {
            IActionResult response;
            try
            {
                response = Ok(await _graphSquadDbService.GetNodes(page));
            }
            catch (Exception e)
            {
                response = UnprocessableEntity(e);
            }

            return response;
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
