using Shared.Models.FootballPlayer;
using Shared.Models.Squaq;
using Shared.Neo4j.DbService;

namespace FutManager.Services;

public interface ISquadService
{
    Task<IEnumerable<Squad>?> GetSquadsForPage(int page);
    Task<int> AddSquad(string name, int balance);
    Task<int> DeleteSquad(int id);
    Task<int> UpdateSquad(Squad squad);
}

public class SquadService : ISquadService
{
    private readonly IGraphDbService<Squad,object?> _graphSquadDbClient;
    private readonly ILogger<SquadService> _logger;

    public SquadService(IGraphDbService<Squad, object?> graphSquadDbClient, ILogger<SquadService> logger)
    {
        _graphSquadDbClient = graphSquadDbClient;
        _logger = logger;
    }

    public async Task<int> AddSquad(string name, int balance)
    {
        try
        {
            var squad = new Squad() 
            {
                Balance = balance,
                Name = name 
            };
            await _graphSquadDbClient.AddNode(squad);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return StatusCodes.Status200OK;
    }
    
    public async Task<int> DeleteSquad(int id)
    { 
        try
        {
            await _graphSquadDbClient.DeleteNode(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return StatusCodes.Status200OK;
    }

    public async Task<IEnumerable<Squad>?> GetSquadsForPage(int page)
    {
        IEnumerable<Squad>? squads = null;
        try
        {
            squads = await _graphSquadDbClient.GetNodes(page);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return squads;
    }

    public async Task<int> UpdateSquad(Squad squad)
    {
        try
        {
            await _graphSquadDbClient.UpdateNode(squad);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return StatusCodes.Status200OK;
    }
}