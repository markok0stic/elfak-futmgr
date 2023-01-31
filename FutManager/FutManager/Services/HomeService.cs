using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Models;
using Shared.Models.FootballPlayer;
using Shared.Models.MatchModels;
using Shared.Models.Squaq;
using Shared.Neo4j.DbService;
using Shared.Redis.Server;
using System.Numerics;

namespace FutManager.Services
{
    public interface IHomeService
    {
        Task<List<CurrentlyLiveMatch>> GetAllActiveMatches();
        Task<List<Player>> GetPlayersForPage(int page);
        Task<List<Squad>> GetSquadsForPage(int page);
        Task<int> AddPlayer(Player player);
        Task<int> AddSquad(string name, int balance);
        Task<int> DeletePlayer(int id);
        Task<int> DeleteSquad(int id);
    }

    public class HomeService : IHomeService
    {
        private readonly IRedisServerClient _redisServerClient;
        private readonly IGraphDbService _graphServerClient;
        private readonly ILogger<HomeService> _logger;

        public HomeService(IRedisServerClient redisServerClient, IGraphDbService graphServerClient, ILogger<HomeService> logger)
        {
            _redisServerClient = redisServerClient;
            _graphServerClient = graphServerClient;
            _logger = logger;
        }

        public async Task<int> AddPlayer(Player player)
        {
            try
            {
                await _graphServerClient.PostPlayer(player);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
            }
            return StatusCodes.Status200OK;
        }

        public async Task<int> AddSquad(string name, int balance)
        {
            try
            {
                await _graphServerClient.PostSquad(name, balance);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
            }
            return StatusCodes.Status200OK;
        }

        public async Task<int> DeletePlayer(int id)
        {
            try
            {
                await _graphServerClient.DeletePlayer(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
            }
            return StatusCodes.Status200OK;
        }

        public async Task<int> DeleteSquad(int id)
        { 
            try
            {
                await _graphServerClient.DeleteSquad(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
            }
            return StatusCodes.Status200OK;
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

        public async Task<List<Player>> GetPlayersForPage(int page)
        {
            var players = new List<Player>();
            try
            {
                players = await _graphServerClient.GetPlayers(page);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
            }
            return players;
        }

        public async Task<List<Squad>> GetSquadsForPage(int page)
        {
            var squads = new List<Squad>();
            try
            {
                squads = await _graphServerClient.GetSquads(page);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
            }
            return squads;
        }
    }
}

