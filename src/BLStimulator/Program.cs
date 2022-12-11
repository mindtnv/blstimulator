using BLStimulator.Infrastructure;
using BLStimulator.Services;
using BLStimulator.Services.Telegram;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IStimulationProvider, StimulationProvider>();
builder.Services.AddScoped<ITelegramUserIdResolver, TelegramUserIdResolver>();
builder.Services.AddScoped<IStimulatorService, TelegramStimulatorService>();
builder.Services.AddScoped<TelegramBotHandler>();
builder.Services.AddSingleton<TelegramBot>();
builder.Services.AddDbContext<TelegramAppContext>(b =>
{
    b.UseSqlite(builder.Configuration.GetConnectionString(nameof(TelegramAppContext)));
});
builder.Services.AddHostedService<TelegramWebhookService>();
builder.Services.AddHostedService<MigrationService>();

builder.Services.Configure<StimulationProvider.StimulationProviderOptions>(
    builder.Configuration.GetSection(nameof(StimulationProvider)));
builder.Services.Configure<TelegramBotOptions>(builder.Configuration.GetSection(nameof(TelegramBot)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseEndpoints(c =>
{
    c.MapControllers();
    c.MapControllerRoute("telegram_bot", app.Services.GetService<TelegramBot>().Route, new
    {
        Controller = "TelegramBot",
        Action = "Post",
    });
});
app.Run();