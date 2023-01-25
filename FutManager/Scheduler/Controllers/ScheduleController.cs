using Microsoft.AspNetCore.Mvc;
using Scheduler.Scheduler.Services;
using Shared.Models.MatchModels;

namespace Scheduler.Controllers;

public class ScheduleController: Controller
{
    private readonly ISchedulerService _schedulerService;
    private readonly ILogger<ScheduleController> _logger;
    public ScheduleController(ISchedulerService schedulerService, ILogger<ScheduleController> logger)
    {
        _schedulerService = schedulerService;
        _logger = logger;
    }

    [HttpGet]
    public void Index()
    {
        
    }

    [HttpPost]
    public async Task<IActionResult> ScheduleMatch([FromBody]Match match)
    {
        await _schedulerService.ScheduleMatch(match);
        _logger.LogInformation($"Match {match.Id}: {match.HomeSquad.Name} vs {match.AwaySquad.Name} is scheduled for: {match.TimeStamp.ToLongDateString()}!");
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> StartPractice([FromBody]Match match)
    {
        await _schedulerService.ScheduleMatch(match);
        _logger.LogInformation($"Match {match.Id}: {match.HomeSquad.Name} vs {match.AwaySquad.Name} just started!");
        return Ok();
    }
}