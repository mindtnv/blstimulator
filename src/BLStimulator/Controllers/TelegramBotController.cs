using BLStimulator.Services.Telegram;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace BLStimulator.Controllers;

public class TelegramBotController : ControllerBase
{
    private readonly TelegramBot _bot;

    public TelegramBotController(TelegramBot bot)
    {
        _bot = bot;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        if (update != null)
            await _bot.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }
}