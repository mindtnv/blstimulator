using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace BLStimulator.Services.Telegram;

public class TelegramWebhookService : IHostedService
{
    private readonly TelegramBot _bot;
    private readonly ILogger<TelegramWebhookService> _logger;

    public TelegramWebhookService(TelegramBot bot, ILogger<TelegramWebhookService> logger)
    {
        _bot = bot;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Set webhook to {Webhook}", _bot.Url);
        await _bot.Client.SetWebhookAsync(_bot.Url, allowedUpdates: new[] {UpdateType.Message},
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete webhook from {Webhook}", _bot.Url);
        await _bot.Client.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}