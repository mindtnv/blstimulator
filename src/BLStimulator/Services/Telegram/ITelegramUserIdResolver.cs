namespace BLStimulator.Services.Telegram;

public interface ITelegramUserIdResolver
{
    Task<long> ResolveUserIdAsync(long userId);
}