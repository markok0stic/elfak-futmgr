namespace Shared.Streaming
{
    /// <summary>
    /// Default Stream publisher interface
    /// </summary>
    public interface IStreamPublisher
    {
        Task PublishAsync<T>(string topic, T data) where T : class;
    }
}

