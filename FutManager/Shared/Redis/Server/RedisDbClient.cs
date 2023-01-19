using Microsoft.Extensions.Options;
using Shared.Models;
using StackExchange.Redis;

namespace Shared.Redis.Server;

public interface IRedisServerClient
{
    Task<List<LiveMatch>> GetActiveMatches();
}

public class RedisServerClient : IRedisServerClient
{
    private readonly IServer _server;
    
    public RedisServerClient(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisOptions> options)
    {
        _server = connectionMultiplexer.GetServer($"{options.Value.Uri}:{options.Value.Port}");
    }
    
    public async Task<List<LiveMatch>> GetActiveMatches()
    {
        var activeMatches = new List<LiveMatch>();
        foreach (var channel in await _server.SubscriptionChannelsAsync())
        {
            var channelString = channel.ToString();
            if(!channelString.Contains("match"))
                continue;
            var item = new LiveMatch();
            item.Channel = channelString;
            activeMatches.Add(item);
        }
        return activeMatches;
    }
}