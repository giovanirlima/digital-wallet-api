using Api.Exceptions;
using Api.Infrastructure.IoC;
using Api.UserEndpoints.v1;
using Api.WalletEndpoints.v1;
using CrossCutting.Settings;
using CrossCutting.Settings.Models;

var builder = WebApplication.CreateBuilder(args);
var isDebug = builder.Environment.IsDevelopment();

#pragma warning disable CS8604
AppSettings.Initialize(
    builder.Configuration.GetSection("Database").Get<Database>(),
    builder.Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>());
#pragma warning restore CS8604

builder.Services.Inject();
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails();

var app = builder.Build();

if (isDebug)
    app.UseSwagger()
        .UseSwaggerUI(s =>
        {
            s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });

app.UseHttpsRedirection();
//app.UseAuthorization();
app.UseExceptionHandler();
app.MapUserEndpoints();
app.MapWalletEndpoints();

app.Run();