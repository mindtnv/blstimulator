namespace BLStimulator.Services.Telegram;

public class TelegramUserIdResolver : ITelegramUserIdResolver
{
    public Task<long> ResolveUserIdAsync(long userId) =>
        Task.FromResult(userId switch
        {
            1 => (long) 499144473,
            _ => throw new Exception(),
        });
}