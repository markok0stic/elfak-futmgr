using Microsoft.Extensions.Options;
using Shared.Models;
using Shared.Models.MatchModels;
using StackExchange.Redis;

namespace Shared.Redis.Server
{
    /// <summary>
    /// Containing some of the definitions for communicating and executing redis-cli commands
    /// </summary>
    public interface IRedisServerClient
    {
        Task<List<CurrentlyLiveMatch>> GetActiveMatches();
    }

    public class RedisServerClient : IRedisServerClient
    {
        private readonly IServer _server;

        public RedisServerClient(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisOptions> options)
        {
            _server = connectionMultiplexer.GetServer($"{options.Value.Uri}:{options.Value.Port}");
        }

        public async Task<List<CurrentlyLiveMatch>> GetActiveMatches()
        {
            var activeMatches = new List<CurrentlyLiveMatch>();
            foreach (var channel in await _server.SubscriptionChannelsAsync())
            {
                var channelString = channel.ToString();
                if (!channelString.Contains("match"))
                    continue;
                var item = new CurrentlyLiveMatch();
                item.Channel = channelString;
                activeMatches.Add(item);
            }
            return activeMatches;
        }
    }
}

