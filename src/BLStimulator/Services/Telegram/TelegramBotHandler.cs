using BLStimulator.Infrastructure;
using BLStimulator.Models;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace BLStimulator.Services.Telegram;

public class TelegramBotHandler
{
    private readonly TelegramAppContext _telegramAppContext;

    public TelegramBotHandler(TelegramAppContext telegramAppContext)
    {
        _telegramAppContext = telegramAppContext;
    }

    public async Task HandleUpdateAsync(Update update, CancellationToken cancellationToken)
    {
        var chatId = update.Message.Chat.Id;
        var containsChatId =
            await _telegramAppContext.ChatIdEntries.AnyAsync(x => x.ChatId == chatId,
                cancellationToken);
        if (!containsChatId)
        {
            await _telegramAppContext.ChatIdEntries.AddAsync(new TelegramChatIdEntry
            {
                ChatId = chatId,
                UserId = update.Message.From.Id,
            }, cancellationToken);
            await _telegramAppContext.SaveChangesAsync(cancellationToken);
        }
    }
}