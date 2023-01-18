using Newtonsoft.Json;
using Shared.Streaming;
using StackExchange.Redis;

namespace Shared.Redis.Streaming;

internal sealed class RedisStreamPublisher : IStreamPublisher
{
    private readonly ISubscriber _subscriber;

    public RedisStreamPublisher(IConnectionMultiplexer connectionMultiplexer)
    {
        _subscriber = connectionMultiplexer.GetSubscriber();
    }
    
    public Task PublishAsync<T>(string topic, T data) where T : class
    {
        var payload = JsonConvert.SerializeObject(data);
        return _subscriber.PublishAsync(topic, payload);
    }
}