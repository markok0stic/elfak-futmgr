using Microsoft.Extensions.Options;
using Shared.Models;
using StackExchange.Redis;

namespace Shared.Redis.Server;

public interface IRedisDbClient
{
    Task<List<LiveMatch>> GetActiveMatches();
}

public class RedisDbClient : IRedisDbClient
{
    private readonly IDatabase _database;
    
    public RedisDbClient(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }
    
    public async Task<List<LiveMatch>> GetActiveMatches()
    {
        var activeMatches = new List<LiveMatch>();
        var a = await _database.ExecuteAsync("PUBSUB CHANNELS");
        /*foreach (var channel in  ))
        {
            var channelString = channel.ToString();
            if(!channelString.Contains("match"))
                continue;
            var item = new LiveMatch();
            item.Channel = channelString;
            activeMatches.Add(item);
        }*/
        return activeMatches;
    }
}