using System.Threading.Channels;
using Aggregator.Listener.Requests;

namespace Aggregator.Listener.Services
{
    internal sealed class AggregatorRequestChannel
    {
        public readonly Channel<IAggregatorRequest> Requests = Channel.CreateUnbounded<IAggregatorRequest>();
    }
}
