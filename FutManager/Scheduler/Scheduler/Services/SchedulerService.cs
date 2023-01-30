using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Scheduler.Models;
using Shared.Models.FootballPlayer;
using Shared.Models.MatchModels;
using Shared.Neo4j.DbService;
using Shared.Neo4j.Enums;
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
    private readonly IGraphDbService<Match,Player?> _graphMatchDbService;
    private readonly IGraphDbService<Player,Match?> _graphPlayerDbService;
    private readonly ILogger<SchedulerService> _logger;

    public SchedulerService(IApiClient apiClient, IOptions<SchedulerOptions> options, ILogger<SchedulerService> logger, IGraphDbService<Match, Player?> graphMatchDbService, IGraphDbService<Player, Match?> graphPlayerDbService)
    {
        _apiClient = apiClient;
        _matchPlayerBaseUrl = options.Value.MatchPlayerBaseUrl;
        _aggregatorBaseUrl = options.Value.AggregatorBaseUrl;
        _logger = logger;
        _graphMatchDbService = graphMatchDbService;
        _graphPlayerDbService = graphPlayerDbService;
    }
    
    public async Task<bool> ScheduleMatch(Match match)
    {
        var res = await StartMatch(match);
        if (res)
        {
            await _graphMatchDbService.AddNode(match);  
        }
        return true;
    }

    private async Task<bool> StartMatch(Match match)
    {
        var result = true;
        var matchPlayerStartUrl = _matchPlayerBaseUrl + "/startMatch";
        var aggregatorStartUrl = _aggregatorBaseUrl + "/aggregateMatch";
        
        var matchPlayerResponse = await _apiClient.PostAsync(matchPlayerStartUrl,
            JsonConvert.SerializeObject(new { match = match }));
        var aggregatorResponse = await _apiClient.PostAsync(aggregatorStartUrl,
            JsonConvert.SerializeObject(new { matchId = match.Id }));
        
        if (matchPlayerResponse == null || aggregatorResponse == null)
        {
            result = false;
            _logger.LogInformation($"Match {match.Id}: A vs B failed to start!");
        }
        else
        {
            _logger.LogInformation($"Match {match.Id}: A vs B just started!");
        }
        
        return result;
    }
}
