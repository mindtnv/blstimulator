using Microsoft.Extensions.Options;

namespace BLStimulator.Services;

public class StimulationProvider : IStimulationProvider
{
    private readonly string _imgPath;

    public StimulationProvider(IOptions<StimulationProviderOptions> _options)
    {
        _imgPath = _options.Value.ImgPath;
    }

    public Task<Stimulation> GetStimulationAsync()
    {
        var images = Directory.GetFiles(_imgPath);
        var text = "WP";
        return Task.FromResult(new Stimulation(text, images[Random.Shared.Next(images.Length)]));
    }

    public class StimulationProviderOptions
    {
        public string ImgPath { get; set; }
    }
}