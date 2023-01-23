using Shared.Models;
using Shared.Models.MatchModels;
using Shared.Redis.Server;

namespace FutManager.Services
{
    public interface IHomeService
    {
        Task<List<CurrentlyLiveMatch>> GetAllActiveMatches();
    }

    public class HomeService : IHomeService
    {
        private readonly IRedisServerClient _redisServerClient;
        private readonly ILogger<HomeService> _logger;

        public HomeService(IRedisServerClient redisServerClient, ILogger<HomeService> logger)
        {
            _redisServerClient = redisServerClient;
            _logger = logger;
        }

        public async Task<List<CurrentlyLiveMatch>> GetAllActiveMatches()
        {
            var liveMatches = new List<CurrentlyLiveMatch>();
            try
            {
                liveMatches = await _redisServerClient.GetActiveMatches();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
            }
            return liveMatches;
        }
    }
}

