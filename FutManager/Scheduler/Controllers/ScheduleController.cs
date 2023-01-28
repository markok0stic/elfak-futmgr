using System.ComponentModel;
using System.Globalization;
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

    [HttpPost]
    public async Task<IActionResult> StartPractice([FromBody]Match match)
    {
        await _schedulerService.ScheduleMatch(match);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> ScheduleMatch([FromBody]Match match)
    {
        var delay = match.TimeStamp - DateTime.Now;
        if (delay.Milliseconds < 0)
        {
            return BadRequest();
        }
        Task.Run(async () =>
        {
            await Task.Delay(delay);
            await _schedulerService.ScheduleMatch(match);
        });
        _logger.LogInformation($"Match {match.Id}: {match.HomeSquad.Name} vs {match.AwaySquad.Name} is scheduled for {match.TimeStamp.ToString(CultureInfo.InvariantCulture)}!");
        await Task.CompletedTask;
        return Ok();
    }
}