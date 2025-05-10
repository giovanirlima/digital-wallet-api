using Api.Behaviors;
using Api.Infrastructure.Profiles;
using CrossCutting.Settings;
using Domain.Interfaces.v1;
using Domain.Services.v1;
using FluentValidation;
using Infrastructure.Data.Command.Commands.v1.Users.AddUser;
using Infrastructure.Data.Command.Interfaces.v1;
using Infrastructure.Data.Command.Repositories.v1;
using Infrastructure.Data.Database.Selectors;
using Infrastructure.Data.Publisher.Publishers.v1;
using Infrastructure.Data.Query.Interfaces.v1;
using Infrastructure.Data.Query.Queries.v1.Users.GetUserByFilters;
using Infrastructure.Data.Query.Repositories.v1;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.IoC;

internal static class Bootstrapper
{
    public static IServiceCollection Inject(this IServiceCollection services)
    {
        InjectMediators(services);
        InjectValidators(services);
        injectAutoMapper(services);
        InjectContexts(services);
        InjectCommandRepositories(services);
        InjectQueryRepositories(services);
        InjectServices(services);
        InjectClients(services);

        return services;
    }

    private static void InjectMediators(IServiceCollection services) =>
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(typeof(AddUserCommand).Assembly, typeof(GetUserByFiltersQuery).Assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggerBehavior<,>));
        });
    private static void InjectValidators(IServiceCollection services) =>
        services.AddValidatorsFromAssemblies([typeof(AddUserCommand).Assembly, typeof(GetUserByFiltersQuery).Assembly]);

    private static void injectAutoMapper(IServiceCollection services) =>
        services.AddAutoMapper(typeof(UserProfile).Assembly);

    private static void InjectCommandRepositories(IServiceCollection services) =>
        services.AddScoped<IUserCommandRepository, UserCommandRepository>();

    private static void InjectQueryRepositories(IServiceCollection services) =>
        services.AddScoped<IUserQueryRepository, UserQueryRepository>();

    private static void InjectContexts(IServiceCollection services) =>
        services.AddDbContextPool<ReadOnlyContext>(opt =>
        {
            opt.UseNpgsql(AppSettings.Database.ReadHost);
        }, 128)
        .AddDbContextPool<ReadWriteContext>(opt =>
        {
            opt.UseNpgsql(AppSettings.Database.WriteHost);
        }, 128);

    private static void InjectClients(IServiceCollection services) =>
        services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();

    private static void InjectServices(IServiceCollection services) =>
        services.AddSingleton<IAuthService, AuthService>();
}