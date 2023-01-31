using Shared.Models.FootballPlayer;
using Shared.Neo4j.DbService;

namespace FutManager.Services;
public interface IPlayerService
{
    Task<IEnumerable<Player>?> GetPlayersForPage(int page);
    Task<int> AddPlayer(Player player);
    Task<int> DeletePlayer(int id);
}
public class PlayerService : IPlayerService
{
    private readonly IGraphDbService<Player,object?> _graphPlayerDbClient;
    private readonly ILogger<PlayerService> _logger;

    public PlayerService(IGraphDbService<Player, object?> graphPlayerDbClient, ILogger<PlayerService> logger)
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
            //await _graphPlayerDbClient.DeletePlayer(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "");
        }
        return StatusCodes.Status200OK;
    }
    
}