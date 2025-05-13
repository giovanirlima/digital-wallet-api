using Digital.Wallet.Settings.Models;

namespace Digital.Wallet.Settings;

public static class AppSettings
{
    public static Database Database { get; set; } = default!;
    public static RabbitMqSettings RabbitMqSettings { get; set; } = default!;
    public static Authenticator Authenticator { get; set; } = default!;

    public static void Initialize(Database database, RabbitMqSettings rabbitMqSettings, Authenticator authenticator)
    {
        Database = database;
        RabbitMqSettings = rabbitMqSettings;
        Authenticator = authenticator;
    }

    public static void Initialize(Database database, RabbitMqSettings rabbitMqSettings)
    {
        Database = database;
        RabbitMqSettings = rabbitMqSettings;
    }
}