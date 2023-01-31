using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Scheduler.Scheduler.Services;
using Shared.Models;
using Shared.Models.DtoModels;
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
    public async Task<IActionResult> StartPractice([FromBody]Match matchDto)
    {
        IActionResult response;
        try
        {
            await _schedulerService.ScheduleMatch(matchDto);
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
    public async Task<IActionResult> ScheduleMatch([FromBody]Match matchDto)
    {
        IActionResult response;
        try
        {
            var delay = matchDto.MatchTime - DateTime.Now;
            if (delay.Milliseconds < 0)
            {
                return BadRequest("Couldn't schedule match...");
            }
            Task.Run(async () =>
            {
                await Task.Delay(delay);
                await _schedulerService.ScheduleMatch(matchDto);
            });
            await Task.CompletedTask;
            _logger.LogInformation($"Match {matchDto.Id}: A vs B is scheduled for {matchDto.MatchTime.ToString(CultureInfo.InvariantCulture)}!");
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