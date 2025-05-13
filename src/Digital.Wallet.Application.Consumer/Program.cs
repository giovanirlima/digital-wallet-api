using Digital.Wallet.Selectors;
using Digital.Wallet.Settings;
using Digital.Wallet.Settings.Models;
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

builder.Services.AddScoped<ITransactionHandler, TransactionHandler>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddTransient<DepositConsumer>();
builder.Services.AddTransient<WithdrawConsumer>();
builder.Services.AddTransient<TransferConsumer>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();