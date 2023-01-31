using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Scheduler.Models;
using Shared.Models;
using Shared.Models.DtoModels;
using Shared.Models.FootballPlayer;
using Shared.Models.MatchModels;
using Shared.Models.SquadModels;
using Shared.Neo4j.DbService;
using Shared.Neo4j.Enums;
using Shared.RestApiClient;

namespace Scheduler.Scheduler.Services;

public interface ISchedulerService
{
    Task ScheduleMatch(Match matchDto);
}

public class SchedulerService: ISchedulerService
{
    private readonly IApiClient _apiClient;
    private readonly string _matchPlayerBaseUrl;
    private readonly string _aggregatorBaseUrl;
    private readonly IGraphDbService<MatchDto,Squad> _graphMatchDbService;
    private readonly ILogger<SchedulerService> _logger;

    public SchedulerService(IApiClient apiClient, IOptions<SchedulerOptions> options, 
        IGraphDbService<MatchDto, Squad> graphMatchDbService, ILogger<SchedulerService> logger)
    {
        _apiClient = apiClient;
        _matchPlayerBaseUrl = options.Value.MatchPlayerBaseUrl;
        _aggregatorBaseUrl = options.Value.AggregatorBaseUrl;
        _logger = logger;
        _graphMatchDbService = graphMatchDbService;
    }
    
    public async Task ScheduleMatch(Match match)
    {
        match.MatchTime = DateTime.Now;
        var matchDto = new MatchDto(match);
        await _graphMatchDbService.AddNode(matchDto);
        await _graphMatchDbService.MakeRelationship(matchDto, match.HomeSquad, RelationshipTypes.HOME);
        await _graphMatchDbService.MakeRelationship(matchDto, match.AwaySquad, RelationshipTypes.AWAY);
        match.Id = matchDto.Id;
        await StartMatch(match);
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
