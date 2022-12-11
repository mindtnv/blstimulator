namespace BLStimulator.Services;

public interface IStimulationProvider
{
    Task<Stimulation> GetStimulationAsync();
}