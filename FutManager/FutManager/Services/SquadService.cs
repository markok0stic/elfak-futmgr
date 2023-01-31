using Shared.Models.DtoModels;
using Shared.Neo4j.DbService;

namespace FutManager.Services;

public interface ISquadService
{
    Task<IEnumerable<SquadDto>?> GetSquadsForPage(int page);
    Task<int> AddSquad(string name, int balance);
    Task<int> DeleteSquad(int id);
    Task<int> UpdateSquad(SquadDto squadDto);
}
/// <summary>
/// Implements all methods needed to interact with squads.
/// Player service contains instance of graphDBService which is used to comunicate with data base,
/// and instance of logger which is used to log errors in console
/// </summary>
public class SquadService : ISquadService
{
    private readonly IGraphDbService<SquadDto,object?> _graphSquadDbClient;
    private readonly ILogger<SquadService> _logger;

    public SquadService(IGraphDbService<SquadDto, object?> graphSquadDbClient, ILogger<SquadService> logger)
    {
        _graphSquadDbClient = graphSquadDbClient;
        _logger = logger;
    }

    public async Task<int> AddSquad(string name, int balance)
    {
        try
        {
            var squad = new SquadDto() 
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

    public async Task<IEnumerable<SquadDto>?> GetSquadsForPage(int page)
    {
        IEnumerable<SquadDto>? squads = null;
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

    public async Task<int> UpdateSquad(SquadDto squadDto)
    {
        try
        {
            await _graphSquadDbClient.UpdateNode(squadDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return StatusCodes.Status200OK;
    }
}