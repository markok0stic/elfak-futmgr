using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Scheduler.Models;
using Shared.Models;
using Shared.Models.DtoModels;
using Shared.Models.MatchModels;
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
    private readonly IGraphDbService<MatchDto,SquadDto> _graphMatchDbService;
    private readonly ILogger<SchedulerService> _logger;

    public SchedulerService(IApiClient apiClient, IOptions<SchedulerOptions> options, 
        IGraphDbService<MatchDto, SquadDto> graphMatchDbService, ILogger<SchedulerService> logger)
    {
        _apiClient = apiClient;
        _matchPlayerBaseUrl = options.Value.MatchPlayerBaseUrl;
        _aggregatorBaseUrl = options.Value.AggregatorBaseUrl;
        _logger = logger;
        _graphMatchDbService = graphMatchDbService;
    }
    
    /// <summary>
    /// Method used to create some of the nodes for Match and its relationships 
    /// </summary>
    /// <param name="match">Match to be started</param>
    public async Task ScheduleMatch(Match match)
    {
        match.MatchTime = DateTime.Now;
        var matchDto = new MatchDto(match);
        await _graphMatchDbService.AddNode(matchDto);
        await _graphMatchDbService.AddRelationship(matchDto.Id, match.HomeSquadDto.Id, RelationshipTypes.HOME);
        await _graphMatchDbService.AddRelationship(matchDto.Id, match.AwaySquadDto.Id, RelationshipTypes.AWAY);
        match.Id = matchDto.Id;
        await StartMatch(match);
    }

    /// <summary>
    /// Method used to execute match player service and aggregator service to start generating and observing data
    /// for match
    /// </summary>
    /// <param name="match">Match to be started</param>
    /// <returns></returns>
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
