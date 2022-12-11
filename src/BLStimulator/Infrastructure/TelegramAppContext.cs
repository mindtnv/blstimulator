using BLStimulator.Models;
using Microsoft.EntityFrameworkCore;

namespace BLStimulator.Infrastructure;

public class TelegramAppContext : DbContext
{
    public DbSet<TelegramChatIdEntry> ChatIdEntries { get; set; }

    protected TelegramAppContext()
    {
    }

    public TelegramAppContext(DbContextOptions options) : base(options)
    {
    }
}