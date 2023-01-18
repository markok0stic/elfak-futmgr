using Newtonsoft.Json;
using Shared.Streaming;
using StackExchange.Redis;

namespace Shared.Redis.Streaming;

internal sealed class RedisStreamSubscriber : IStreamSubscriber
{
    private readonly ISubscriber _subscriber;

    public RedisStreamSubscriber(IConnectionMultiplexer connectionMultiplexer)
    {
        _subscriber = connectionMultiplexer.GetSubscriber();
    }

    public Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class
    {
        return _subscriber.SubscribeAsync(topic, (_, data) =>
        {
            var payload = JsonConvert.DeserializeObject<T>(data);
            if (payload is null)
            {
                return;
            }

            handler(payload);
        });
    }
}