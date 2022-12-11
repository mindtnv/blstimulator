namespace BLStimulator.Services;

public interface IStimulatorService
{
    Task StimulateAsync(long userId);
}