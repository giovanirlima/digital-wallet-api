using Api.Behaviors;
using CrossCutting.Settings;
using FluentValidation;
using Infrastructure.Data.Command.Commands.v1.Users.AddUser;
using Infrastructure.Data.Command.Interfaces.v1;
using Infrastructure.Data.Command.Repositories.v1;
using Infrastructure.Data.Database.Selectors;
using Infrastructure.Data.Query.Interfaces.v1;
using Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;
using Infrastructure.Data.Query.Repositories.v1;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Api.Infrastructure.IoC;

public class Bootstrapper(IServiceCollection services)
{
    private readonly IServiceCollection _services = services;

    private readonly Assembly[] _assemblies =
    [
        typeof(AddUserCommand).Assembly,
        typeof(GetUserByFiltersQuery).Assembly,
        typeof(Program).Assembly
    ];

    public void Inject(bool isDebug)
    {
        InjectMediatorsDependencies();
        InjectValidatorsDependencies();
        injectAutoMapper();
        InjectContexts(isDebug);
        InjectRepositoriesDependencies();
    }

    private void InjectMediatorsDependencies() =>
        _services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(_assemblies);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggerBehavior<,>));
        });

    private void InjectValidatorsDependencies() =>
        _services.AddValidatorsFromAssemblies(_assemblies);

    private void injectAutoMapper() =>
        _services.AddAutoMapper(_assemblies);

    private void InjectRepositoriesDependencies()
    {
        _services.AddScoped<IUserQueryRepository, UserQueryRepository>();
        _services.AddScoped<IUserCommandRepository, UserCommandRepository>();
    }

    private void InjectContexts(bool isDebug)
    {
        _services.AddDbContextPool<ReadOnlyContext>(opt =>
        {
            opt.UseNpgsql(AppSettings.Database.ReadHost);

            if (isDebug)
            {
                opt.EnableSensitiveDataLogging(true);
                opt.LogTo(Console.WriteLine, LogLevel.Information);
            }
        }, 128);

        _services.AddDbContextPool<ReadWriteContext>(opt =>
        {
            opt.UseNpgsql(AppSettings.Database.WriteHost);

            if (isDebug)
            {
                opt.EnableSensitiveDataLogging(true);
                opt.LogTo(Console.WriteLine, LogLevel.Information);
            }
        }, 128);
    }
}