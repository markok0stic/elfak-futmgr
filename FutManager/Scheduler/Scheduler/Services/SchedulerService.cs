using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Scheduler.Models;
using Shared.Models.MatchModels;
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

    public SchedulerService(IApiClient apiClient, IOptions<SchedulerOptions> options)
    {
        _apiClient = apiClient;
        _matchPlayerBaseUrl = options.Value.MatchPlayerBaseUrl;
        _aggregatorBaseUrl = options.Value.AggregatorBaseUrl;
    }


    public async Task<bool> ScheduleMatch(Match match)
    {
        var result = true;
        var matchPlayerStartUrl = _matchPlayerBaseUrl + "/startMatch";
        var aggregatorStartUrl = _aggregatorBaseUrl + "/aggregateMatch";
        
        var matchPlayerResponse = await _apiClient.PostAsync(matchPlayerStartUrl,
            JsonConvert.SerializeObject(new { match }));
        var aggregatorResponse = await _apiClient.PostAsync(aggregatorStartUrl,
            JsonConvert.SerializeObject(new { matchId = match.Id }));
        
        if (matchPlayerResponse == null || aggregatorResponse == null)
            result = false;
        return result;
    }

    public async Task<bool> StopMatch(Match match)
    {
        throw new NotImplementedException();
    }
}
