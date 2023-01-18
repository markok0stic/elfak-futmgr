using Shared.Models;
using Shared.Redis.Server;

namespace FutManager.Services;

public interface IHomeService
{
    Task<List<LiveMatch>> GetAllActiveMatches();
}

public class HomeService: IHomeService
{
    private readonly IRedisDbClient _redisDbClient;
    private readonly ILogger<HomeService> _logger;

    public HomeService(IRedisDbClient redisDbClient, ILogger<HomeService> logger)
    {
        _redisDbClient = redisDbClient;
        _logger = logger;
    }

    public async Task<List<LiveMatch>> GetAllActiveMatches()
    {
        var liveMatches = new List<LiveMatch>();
        try
        {
            liveMatches = await _redisDbClient.GetActiveMatches();
        }
        catch (Exception e)
        {
           _logger.LogError(e,"");
        }
        return liveMatches;
    }
}