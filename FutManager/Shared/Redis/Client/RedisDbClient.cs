using Shared.Models;
using StackExchange.Redis;

namespace Shared.Redis.Client;

public interface IRedisDbClient
{
    
}

public class RedisDbClient : IRedisDbClient
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    public RedisDbClient(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public List<Match> GetActiveMatches()
    {
        return new List<Match>();
    }
}