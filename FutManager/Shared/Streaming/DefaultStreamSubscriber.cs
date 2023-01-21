namespace Shared.Streaming
{
    internal sealed class DefaultStreamSubscriber : IStreamSubscriber
    {
        public Task SubscribeAsync<T>(string topic, Action<T> handler) where T : class => Task.CompletedTask;
        public Task UnsubscribeAsync(string topic) => Task.CompletedTask;
    }
}

