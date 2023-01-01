using BLStimulator.Infrastructure;
using BLStimulator.Models;
using GBMSTelegramBotFramework.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BLStimulator.Services.Telegram;

public class TelegramBotHandler : UpdateHandlerBase
{
    private readonly TelegramAppContext _telegramAppContext;

    public TelegramBotHandler(TelegramAppContext telegramAppContext)
    {
        _telegramAppContext = telegramAppContext;
    }

    public override async Task OnMessageAsync(UpdateContext context)
    {
        var update = context.Update;
        var chatId = update.Message.Chat.Id;
        var containsChatId =
            await _telegramAppContext.ChatIdEntries.AnyAsync(x => x.ChatId == chatId);
        if (!containsChatId)
        {
            await _telegramAppContext.ChatIdEntries.AddAsync(new TelegramChatIdEntry
            {
                ChatId = chatId,
                UserId = update.Message.From.Id,
            });
            await _telegramAppContext.SaveChangesAsync();
        }
    }
}