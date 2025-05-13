using Digital.Wallet.Behaviors;
using Digital.Wallet.Commands.v1.Users.AddUser;
using Digital.Wallet.Infrastructure.Profiles;
using Digital.Wallet.Interfaces.v1;
using Digital.Wallet.Publishers.v1;
using Digital.Wallet.Queries.v1.Users.GetUserByFilters;
using Digital.Wallet.Repositories.v1;
using Digital.Wallet.Selectors;
using Digital.Wallet.Services.v1;
using Digital.Wallet.Settings;
using FluentValidation;
using Infrastructure.Data.Query.Repositories.v1;
using Microsoft.EntityFrameworkCore;

namespace Digital.Wallet.Infrastructure.IoC;

internal static class Bootstrapper
{
    public static IServiceCollection Inject(this IServiceCollection services)
    {
        InjectMediators(services);
        InjectValidators(services);
        injectAutoMapper(services);
        InjectContexts(services);
        InjectRepositories(services);
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

    private static void InjectRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<ITransactionReadRepository, TransactionReadRepository>();
    }

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