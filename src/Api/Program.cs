using Api.Exceptions;
using Api.Infrastructure.IoC;
using Api.UserMaps.v1;
using CrossCutting.Settings;
using CrossCutting.Settings.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var isDebug = builder.Environment.IsDevelopment();
var bootstrapper = new Bootstrapper(services);

#pragma warning disable CS8604
AppSettings.Initialize(
    configuration.GetSection("Database").Get<Database>());
#pragma warning restore CS8604

bootstrapper.Inject(isDebug);
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();

if (isDebug)
    app.UseSwagger()
       .UseSwaggerUI(s =>
       {
           s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
           s.RoutePrefix = string.Empty;
       });

app.UseHttpsRedirection();
//app.UseAuthorization();
app.UseExceptionHandler(opt => { });
app.MapUserEndpoints();

app.Run();