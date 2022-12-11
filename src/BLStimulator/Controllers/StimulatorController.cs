using BLStimulator.Services;
using Microsoft.AspNetCore.Mvc;

namespace BLStimulator.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class StimulatorController : ControllerBase
{
    private readonly IStimulatorService _stimulatorService;

    public StimulatorController(IStimulatorService stimulatorService)
    {
        _stimulatorService = stimulatorService;
    }

    [HttpGet]
    [Route("/stimulate")]
    public async Task<IResult> Stimulate([FromQuery] long userId, [FromQuery] int rewardsCount = 1)
    {
        for (var i = 0; i < rewardsCount; i++)
            await _stimulatorService.StimulateAsync(userId);

        return Results.Ok();
    }
}