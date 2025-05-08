using Api.Behaviors;
using Api.Exceptions;
using Api.UserMaps.v1;
using CrossCutting.Settings;
using CrossCutting.Settings.Models;
using FluentValidation;
using Infrastructure.Data.Command.Commands.v1.Users.AddUser;
using Infrastructure.Data.Database.Selectors;
using Infrastructure.Data.Query.Interfaces.v1;
using Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;
using Infrastructure.Data.Query.Repositories.v1;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var isDebug = builder.Environment.IsDevelopment();

var assemblies = new[] {
    typeof(AddUserCommand).Assembly,
    typeof(GetUserByFiltersQuery).Assembly
};

#pragma warning disable CS8604
AppSettings.Initialize(
    configuration.GetSection("Database").Get<Database>());
#pragma warning restore CS8604

services.AddDbContextPool<ReadOnlyContext>(opt =>
{
    var connectionString =
        $"Server={AppSettings.Database.ReadHost};" +
        $"Database={AppSettings.Database.Base};" +
        $"User Id={AppSettings.Database.User};" +
        $"Password={AppSettings.Database.Password}";

    opt.UseNpgsql(connectionString);

    if (isDebug)
    {
        opt.EnableSensitiveDataLogging(true);
        opt.LogTo(Console.WriteLine, LogLevel.Information);
    }
}, 128);

services.AddDbContextPool<ReadWriteContext>(opt =>
{
    var connectionString =
        $"Server={AppSettings.Database.ReadHost};" +
        $"Database={AppSettings.Database.Base};" +
        $"User Id={AppSettings.Database.User};" +
        $"Password={AppSettings.Database.Password}";

    opt.UseNpgsql(connectionString);

    if (isDebug)
    {
        opt.EnableSensitiveDataLogging(true);
        opt.LogTo(Console.WriteLine, LogLevel.Information);
    }
}, 128);

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddValidatorsFromAssemblies(assemblies);
services.AddExceptionHandler<GlobalExceptionHandler>();

services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assemblies);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggerBehavior<,>));
});

services.AddSingleton<IUserQueryRepository, UserQueryRepository>();

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