using Microsoft.AspNetCore.SignalR;
using Shared.Models;
using Shared.Streaming;

namespace FutManager.Services.RedisHub;

public sealed class RedisHub : Hub
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
        await _subscriber.SubscribeAsync<MatchResult>(channel, sub =>
        {
            _logger.LogInformation("Winner is {ObjWinner} and TS: {ObjTimeStamp }", sub.Winner, sub.TimeStamp); ;
            Clients.Group(channel).SendAsync("ReceiveScore", sub);
        });
    }
    
}