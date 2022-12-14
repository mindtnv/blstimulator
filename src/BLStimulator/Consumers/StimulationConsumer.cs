using BLStimulator.Contracts.Commands;
using BLStimulator.Services;
using MassTransit;

namespace BLStimulator.Consumers;

public class StimulationConsumer : IConsumer<StimulateUser>
{
    private readonly IStimulatorService _stimulatorService;

    public StimulationConsumer(IStimulatorService stimulatorService)
    {
        _stimulatorService = stimulatorService;
    }

    public Task Consume(ConsumeContext<StimulateUser> context)
    {
        var command = context.Message;
        var tasks = new Task[command.Count];
        for (var i = 0; i < command.Count; i++)
            tasks[i] = _stimulatorService.StimulateAsync(command.UserId);
        return Task.WhenAll(tasks);
    }
}