using System.Collections.Concurrent;
using Newtonsoft.Json;
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
        private readonly int _messageDelay;

        public LiveMessageGenerator(ILogger<LiveMessageGenerator> logger, IConfiguration configuration)
        {
            _logger = logger;
            _matchRunningStatus = new ConcurrentDictionary<int, bool>();
            _messageDelay = configuration.GetValue<int>("Match:MessageDelay");
        }

        public async IAsyncEnumerable<MatchLiveMessage> StartAsync(Match match)
        {
            _matchRunningStatus[match.Id] = true;
            var liveMessage = new MatchLiveMessage()
            {
                Id = match.Id,
                AwaySquad = match.AwaySquad,
                HomeSquad = match.HomeSquad,
                Card = null,
                Message = "",
                Result = null,
                Score = null,
                Scores = match.Scores,
                TimeStamp = match.TimeStamp,
                Minute = 0
            };
            var i = 0;
            while (_matchRunningStatus.TryGetValue(match.Id, out var isRunning) && isRunning && i <= 90)
            {
                i++;
                liveMessage.Message = "Hello World";
                liveMessage.Minute++;
                if (i == 90)
                {
                    liveMessage.Result = "1";
                    _logger.LogInformation("Match ended");
                }
                _logger.LogInformation(JsonConvert.SerializeObject(liveMessage));
                yield return liveMessage;
                await Task.Delay(_messageDelay);
            }
        }

        public Task StopAsync(int matchId)
        {
            _matchRunningStatus[matchId] = false;
            return Task.CompletedTask;
        }
    }
}
