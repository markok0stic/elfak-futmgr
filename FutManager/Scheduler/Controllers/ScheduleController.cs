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
        IActionResult response;
        try
        {
            await _schedulerService.ScheduleMatch(match);
            response = Ok("Match successfully started.");
        }
        catch (Exception e)
        {
           _logger.LogError(e,"");
           response = UnprocessableEntity(e);
        }
        return response;
    }

    [HttpPost]
    public async Task<IActionResult> ScheduleMatch([FromBody]Match match)
    {
        IActionResult response;
        try
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
            await Task.CompletedTask;
            _logger.LogInformation($"Match {match.Id}: A vs B is scheduled for {match.MatchTime.ToString(CultureInfo.InvariantCulture)}!");
            response = Ok("Match successfully scheduled.");
        }
        catch (Exception e)
        {
            _logger.LogError(e,"");
            response = UnprocessableEntity(e);
        }

        return response;
    }
}