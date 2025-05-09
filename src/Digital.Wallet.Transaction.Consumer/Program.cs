using CrossCutting.Settings;
using CrossCutting.Settings.Models;
using Digital.Wallet.Transaction.Consumer;
using Infrastructure.Data.Database.Selectors;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

#pragma warning disable CS8604
AppSettings.Initialize(
    builder.Configuration.GetSection("Database").Get<Database>(),
    builder.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>());
#pragma warning restore CS8604

builder.Services.AddDbContextPool<ReadWriteContext>(opt =>
{
    opt.UseNpgsql(AppSettings.Database.WriteHost);
}, 128);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();