using BLStimulator.Consumers;
using BLStimulator.Infrastructure;
using BLStimulator.Services;
using BLStimulator.Services.Telegram;
using GBMSTelegramBotFramework.Abstractions.Extensions;
using GBMSTelegramBotFramework.AspNetCore;
using GBMSTelegramBotFramework.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables("APP_");
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddMassTransit(b =>
{
    b.AddConsumer<StimulationConsumer>();
    b.UsingRabbitMq((ctx, c) =>
    {
        var config = builder.Configuration.GetSection("RabbitMQ");
        c.Host(config["Host"], config["VHost"], x =>
        {
            x.Username(config["User"]);
            x.Password(config["Password"]);
        });

        c.ConfigureEndpoints(ctx);
    });
});
builder.Services.UseTelegramWebHook(o =>
    o.Url = builder.Configuration[ConfigurationPath.Combine("TelegramBot", "Url")]);
builder.Services.AddTelegramBot(bot =>
{
    bot.ConfigureOptions(o =>
    {
        o.WithName("stimulator-bot");
        o.WithToken(builder.Configuration[ConfigurationPath.Combine("TelegramBot", "Token")] ??
                    throw new ArgumentNullException("TelegramBot:Token"));
    });
    bot.UseHandler<TelegramBotHandler>();
});
builder.Services.AddScoped<IStimulationProvider, StimulationProvider>();
builder.Services.AddScoped<ITelegramUserIdResolver, TelegramUserIdResolver>();
builder.Services.AddScoped<IStimulatorService, TelegramStimulatorService>();

builder.Services.AddScoped<TelegramBotHandler>();
builder.Services.AddDbContext<TelegramAppContext>(b =>
{
    b.UseSqlite(builder.Configuration.GetConnectionString(nameof(TelegramAppContext)));
});
builder.Services.AddHostedService<MigrationService>();
builder.Services.Configure<StimulationProviderOptions>(builder.Configuration.GetSection(nameof(StimulationProvider)));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseTelegramWebhook();
app.Run();