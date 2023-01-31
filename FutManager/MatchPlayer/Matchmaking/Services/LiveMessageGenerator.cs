using System.Collections.Concurrent;
using Newtonsoft.Json;
using Shared.Models;
using Shared.Models.DtoModels;
using Shared.Models.FootballPlayer;
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
            var i = 0;
            while (_matchRunningStatus.TryGetValue(match.Id, out var isRunning) && isRunning && i <= 90)
            {
                i++;
                var liveMessage = new MatchLiveMessage()
                {
                    Id = match.Id,
                    Result = null,
                    Score = null,
                    MatchTime = match.MatchTime,
                    Minute = i,
                    HomeSquad = match.HomeSquad,
                    AwaySquad = match.AwaySquad
                };
                
                
                // those are some custom made random match cases 
                liveMessage.Message = "Hello World";
                if (i == 90)
                {
                    liveMessage.Result = "x";
                    _logger.LogInformation("Match ended");
                }

                if (i == 35)
                {
                    liveMessage.Score = new Player() { FirstName = "Dusan", LastName = "Vlahovic" };
                }
                
                if (i == 40)
                {
                    liveMessage.Score = new Player() { FirstName = "Dusan", LastName = "Vlahovic" };
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
