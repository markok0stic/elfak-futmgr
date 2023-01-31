using Shared.Models.FootballPlayer;
using Shared.Models.SquadModels;
using Shared.Neo4j.DbService;

namespace FutManager.Services;
public interface IPlayerService
{
    Task<IEnumerable<Player>?> GetPlayersForPage(int page);
    Task<int> AddPlayer(Player player);
    Task<int> DeletePlayer(int id);
    Task<Player?> GetPlayer(int id);
    Task<int> UpdatePlayer(Player player);
    public Task<int> AddPlayerToSquad(int SquadId, int PlayerId);
}
public class PlayerService : IPlayerService
{
    private readonly IGraphDbService<Player, Squad> _graphPlayerDbClient;
    private readonly ILogger<PlayerService> _logger;

    public PlayerService(IGraphDbService<Player, Squad> graphPlayerDbClient, ILogger<PlayerService> logger)
    {
        _graphPlayerDbClient = graphPlayerDbClient;
        _logger = logger;
    }
    
    public async Task<IEnumerable<Player>?> GetPlayersForPage(int page)
    {
        IEnumerable<Player>? players = null;
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
    
    public async Task<int> AddPlayer(Player player)
    {
        try
        {
            await _graphPlayerDbClient.AddNode(player);
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

    public async Task<Player?> GetPlayer(int id)
    {
        Player? player = null;
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

    public async Task<int> UpdatePlayer(Player player)
    {
        try
        {
            await _graphPlayerDbClient.UpdateNode(player);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return StatusCodes.Status200OK;
    }

    public async Task<int> AddPlayerToSquad(int SquadId, int PlayerId)
    {
        var squad = new Squad()
        {
            Id = SquadId,
        };
        var player = new Player()
        {
            Id = PlayerId
        };
        try
        {
            await _graphPlayerDbClient.MakeRelationship(player, squad, Shared.Neo4j.Enums.RelationshipTypes.PLAYES_FOR);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return StatusCodes.Status200OK;
    }
}