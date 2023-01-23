using Microsoft.AspNetCore.SignalR;
using Shared.Models;
using Shared.Models.MatchModels;
using Shared.Streaming;

namespace FutManager.Hubs
{
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
            await Groups.AddToGroupAsync(Context.ConnectionId, channel);
            await _subscriber.SubscribeAsync<MatchLiveMessage>(channel, sub =>
            {
                /*if (sub.Winner == channel)
                {
                    _subscriber.UnsubscribeAsync(channel);
                }*/
                Clients.Group(channel).SendAsync("ReceiveMessage", sub);
            });
        }

    }
}

