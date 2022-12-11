using BLStimulator.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BLStimulator.Services;

public class MigrationService : IHostedService
{
    private readonly ILogger<MigrationService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public MigrationService(IServiceProvider serviceProvider, ILogger<MigrationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Migrating database...");
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TelegramAppContext>();
        await context.Database.MigrateAsync(cancellationToken);
        _logger.LogInformation("Database migration completed");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}