using Shared.Serialization;
using Shared.Streaming;
using StackExchange.Redis;

namespace Shared.Redis.Streaming;

internal sealed class RedisStreamPublisher : IStreamPublisher
{
    private readonly ISerializer _serializer;
    private readonly ISubscriber _subscriber;

    public RedisStreamPublisher(IConnectionMultiplexer connectionMultiplexer, ISerializer serializer)
    {
        _serializer = serializer;
        _subscriber = connectionMultiplexer.GetSubscriber();
    }
    
    public Task PublishAsync<T>(string topic, T data) where T : class
    {
        try
        {
            // TODO: this one is not working well
            var payload = _serializer.Serialize(data);
            return _subscriber.PublishAsync(topic, payload);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}