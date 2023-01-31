using Microsoft.AspNetCore.SignalR;
using Shared.Models.MatchModels;
using Shared.Streaming;

namespace FutManager.Hubs
{
    /// <summary>
    /// Hub used to stream data using socket communication for client-serving match live messages of desired match
    /// </summary>
    internal sealed class RedisHub : Hub
    {
        private readonly IStreamSubscriber _subscriber;
        private readonly ILogger<RedisHub> _logger;

        public RedisHub(IStreamSubscriber subscriber, ILogger<RedisHub> logger)
        {
            _subscriber = subscriber;
            _logger = logger;
        }

        public async Task StreamMatches(string channel)
        {
            try
            {
                await StopStreaming(channel);
                await Groups.AddToGroupAsync(Context.ConnectionId, channel);
                await _subscriber.SubscribeAsync<MatchLiveMessage>(channel, sub =>
                {
                    if (sub.Result != null)
                    {
                        _subscriber.UnsubscribeAsync(channel);
                    }
                    Clients.Group(channel).SendAsync("ReceiveMessage", sub);
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e,"");
            }
        }

        public async Task StopStreaming(string channel)
        {
            await _subscriber.UnsubscribeAsync(channel);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channel);
        }
    }
}

