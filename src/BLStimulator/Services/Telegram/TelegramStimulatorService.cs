using BLStimulator.Infrastructure;
using GBMSTelegramBotFramework.Abstractions;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

namespace BLStimulator.Services.Telegram;

public class TelegramStimulatorService : IStimulatorService
{
    private readonly IBot _bot;
    private readonly IStimulationProvider _stimulationProvider;
    private readonly TelegramAppContext _telegramAppContext;
    private readonly ITelegramUserIdResolver _telegramUserIdResolver;

    public TelegramStimulatorService(IStimulationProvider stimulationProvider, TelegramAppContext telegramAppContext,
        ITelegramUserIdResolver telegramUserIdResolver, IBot bot)
    {
        _stimulationProvider = stimulationProvider;
        _telegramAppContext = telegramAppContext;
        _telegramUserIdResolver = telegramUserIdResolver;
        _bot = bot;
    }

    public async Task StimulateAsync(long userId)
    {
        var stimulation = await _stimulationProvider.GetStimulationAsync();
        var telegramUserId = await _telegramUserIdResolver.ResolveUserIdAsync(userId);
        var entry = await _telegramAppContext.ChatIdEntries.FirstAsync(x => x.UserId == telegramUserId);
        var img = File.OpenRead(stimulation.ImagePath);
        var message = await _bot.Client.SendPhotoAsync(entry.ChatId, new InputMedia(img, stimulation.ImagePath),
            stimulation.Text);

        Task.Run(async () =>
        {
            await Task.Delay(Random.Shared.Next(8000, 12000));
            await _bot.Client.DeleteMessageAsync(entry.ChatId, message.MessageId);
        });
    }
}