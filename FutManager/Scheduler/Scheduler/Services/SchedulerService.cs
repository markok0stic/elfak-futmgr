using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Scheduler.Models;
using Shared.Models.MatchModels;
using Shared.Neo4j.DbService;
using Shared.RestApiClient;

namespace Scheduler.Scheduler.Services;

public interface ISchedulerService
{
    Task<bool> ScheduleMatch(Match match);
}

public class SchedulerService: ISchedulerService
{
    private readonly IApiClient _apiClient;
    private readonly string _matchPlayerBaseUrl;
    private readonly string _aggregatorBaseUrl;
    private readonly IGraphDbService _graphDbService;
    private readonly ILogger<SchedulerService> _logger;

    public SchedulerService(IApiClient apiClient, IOptions<SchedulerOptions> options, IGraphDbService graphDbService, ILogger<SchedulerService> logger)
    {
        _apiClient = apiClient;
        _matchPlayerBaseUrl = options.Value.MatchPlayerBaseUrl;
        _aggregatorBaseUrl = options.Value.AggregatorBaseUrl;
        _graphDbService = graphDbService;
        _logger = logger;
    }
    
    public async Task<bool> ScheduleMatch(Match match)
    {
        var res = await StartMatch(match);
        if (res)
        {
            await _graphDbService.InsertMatch(match);
        }

        return res;
    }

    private async Task<bool> StartMatch(Match match)
    {
        var result = true;
        var matchPlayerStartUrl = _matchPlayerBaseUrl + "/startMatch";
        var aggregatorStartUrl = _aggregatorBaseUrl + "/aggregateMatch";
        
        var matchPlayerResponse = await _apiClient.PostAsync(matchPlayerStartUrl,
            JsonConvert.SerializeObject(new { match }));
        var aggregatorResponse = await _apiClient.PostAsync(aggregatorStartUrl,
            JsonConvert.SerializeObject(new { matchId = match.Id }));
        
        if (matchPlayerResponse == null || aggregatorResponse == null)
        {
            result = false;
            _logger.LogInformation($"Match {match.Id}: {match.HomeSquad.Name} vs {match.AwaySquad.Name} failed to start!");
        }
        else
        {
            _logger.LogInformation($"Match {match.Id}: {match.HomeSquad.Name} vs {match.AwaySquad.Name} just started!");
        }
        
        return result;
    }
}
