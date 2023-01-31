using Shared.Models.DtoModels;
using Shared.Neo4j.DbService;

namespace FutManager.Services;
public interface IPlayerService
{
    Task<IEnumerable<PlayerDto>?> GetPlayersForPage(int page);
    Task<int> AddPlayer(PlayerDto playerDto);
    Task<int> DeletePlayer(int id);
    Task<PlayerDto?> GetPlayer(int id);
    Task<int> UpdatePlayer(PlayerDto playerDto);
    public Task<int> AddPlayerToSquad(int squadId, int playerId);
}
public class PlayerService : IPlayerService
{
    private readonly IGraphDbService<PlayerDto, SquadDto> _graphPlayerDbClient;
    private readonly ILogger<PlayerService> _logger;

    public PlayerService(IGraphDbService<PlayerDto, SquadDto> graphPlayerDbClient, ILogger<PlayerService> logger)
    {
        _graphPlayerDbClient = graphPlayerDbClient;
        _logger = logger;
    }
    
    public async Task<IEnumerable<PlayerDto>?> GetPlayersForPage(int page)
    {
        IEnumerable<PlayerDto>? players = null;
        try
        {
            players = await _graphPlayerDbClient.GetNodes(page);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return players;
    }
    
    public async Task<int> AddPlayer(PlayerDto playerDto)
    {
        try
        {
            await _graphPlayerDbClient.AddNode(playerDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return StatusCodes.Status200OK;
    }
    
    public async Task<int> DeletePlayer(int id)
    {
        try
        {
            await _graphPlayerDbClient.DeleteNode(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return StatusCodes.Status200OK;
    }

    public async Task<PlayerDto?> GetPlayer(int id)
    {
        PlayerDto? player = null;
        try
        {
            player = await _graphPlayerDbClient.GetNode(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return player;
    }

    public async Task<int> UpdatePlayer(PlayerDto playerDto)
    {
        try
        {
            await _graphPlayerDbClient.UpdateNode(playerDto);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return StatusCodes.Status200OK;
    }

    public async Task<int> AddPlayerToSquad(int squadId, int playerId)
    {
        try
        {
            await _graphPlayerDbClient.AddRelationship(playerId, squadId, Shared.Neo4j.Enums.RelationshipTypes.PLAYES_FOR);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return StatusCodes.Status200OK;
    }
}