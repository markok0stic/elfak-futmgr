namespace Shared.Streaming
{
    /// <summary>
    /// Default Stream subscriber interface
    /// </summary>
    public interface IStreamSubscriber
    {
        Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class;
        Task UnsubscribeAsync(string topic);
    }
}

