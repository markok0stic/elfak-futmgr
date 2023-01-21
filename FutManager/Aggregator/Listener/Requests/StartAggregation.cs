namespace Aggregator.Listener.Requests
{
    internal record StartAggregation : IAggregatorRequest
    {
        public int MatchId { get; set; }
    }
}

