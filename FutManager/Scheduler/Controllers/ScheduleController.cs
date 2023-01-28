using System.ComponentModel;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Scheduler.Services;
using Shared.Models.MatchModels;
using Shared.Neo4j.DbService;

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
        IActionResult response = Ok("Match successfully started.");
        var successfullyStarted = await _schedulerService.ScheduleMatch(match);
        if (!successfullyStarted)
        {
            response = BadRequest("There was an error in starting this match...");
        }
        return response;
    }

    [HttpPost]
    public async Task<IActionResult> ScheduleMatch([FromBody]Match match)
    {
        var delay = match.MatchTime - DateTime.Now;
        if (delay.Milliseconds < 0)
        {
            return BadRequest("Couldn't schedule match...");
        }
        Task.Run(async () =>
        {
            await Task.Delay(delay);
            await _schedulerService.ScheduleMatch(match);
        });
        _logger.LogInformation($"Match {match.Id}: {match.HomeSquad.Name} vs {match.AwaySquad.Name} is scheduled for {match.MatchTime.ToString(CultureInfo.InvariantCulture)}!");
        await Task.CompletedTask;
        return Ok("Match successfully scheduled.");
    }
}