using Microsoft.Extensions.Options;

namespace BLStimulator.Services;

public class StimulationProvider : IStimulationProvider
{
    private readonly string _imgPath;
    private readonly string _textPath;

    public StimulationProvider(IOptions<StimulationProviderOptions> _options)
    {
        _imgPath = _options.Value.ImgPath;
        _textPath = _options.Value.TextPath;
    }

    public Task<Stimulation> GetStimulationAsync()
    {
        var images = Directory.GetFiles(_imgPath);
        var lines = File.ReadAllLines(_textPath);
        var text = lines[Random.Shared.Next(lines.Length)];
        return Task.FromResult(new Stimulation(text, images[Random.Shared.Next(images.Length)]));
    }
}