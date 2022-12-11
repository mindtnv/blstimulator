using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BLStimulator.Services.Telegram;

public class TelegramBot
{
    private readonly IServiceProvider _serviceProvider;
    public TelegramBotOptions Options { get; }
    public TelegramBotClient Client { get; }
    public string Route { get; }
    public string Url { get; }
    public string Secret { get; }

    public TelegramBot(IHttpClientFactory httpClientFactory, IOptions<TelegramBotOptions> options,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Options = options.Value;
        Secret = Path.GetRandomFileName()[1..5];
        Route = $"bot/{Secret}";
        Url = new Uri(new Uri(Options.Url), Route).ToString();
        Client = new TelegramBotClient(Options.Token ?? throw new Exception("Token is null"),
            httpClientFactory.CreateClient());
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetService<TelegramBotHandler>();
        await handler.HandleUpdateAsync(update, cancellationToken);
    }
}