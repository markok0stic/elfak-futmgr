using System.Collections.Concurrent;
using Shared.Models;
using Shared.Models.MatchModels;

namespace MatchPlayer.Matchmaking.Services
{
    internal interface ILiveMessageGenerator
    {
        IAsyncEnumerable<MatchLiveMessage> StartAsync(Match match);
        Task StopAsync(int matchId);
    }
    internal sealed class LiveMessageGenerator : ILiveMessageGenerator
    {
        private readonly ConcurrentDictionary<int, bool> _matchRunningStatus;
        private readonly ILogger<ILiveMessageGenerator> _logger;
        private readonly int _matchDuration;

        public LiveMessageGenerator(ILogger<LiveMessageGenerator> logger, IConfiguration configuration)
        {
            _logger = logger;
            _matchRunningStatus = new ConcurrentDictionary<int, bool>();
            _matchDuration = configuration.GetValue<int>("Match:Duration");
        }

        public async IAsyncEnumerable<MatchLiveMessage> StartAsync(Match match)
        {
            _matchRunningStatus[match.Id] = true;
            var i = 0;
            while (_matchRunningStatus.TryGetValue(match.Id, out var isRunning) && isRunning && i <= 90)
            {
                var liveMessage = new MatchLiveMessage();
                liveMessage.MatchId = match.Id;
                _logger.LogInformation(liveMessage.Message);
                yield return liveMessage;
                i++;
                await Task.Delay(_matchDuration*1000);
            }
        }

        public Task StopAsync(int matchId)
        {
            _matchRunningStatus[matchId] = false;
            return Task.CompletedTask;
        }
    }
}
