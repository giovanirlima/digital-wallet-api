using CrossCutting.Settings.Models;

namespace CrossCutting.Settings;

public static class AppSettings
{
    public static Database Database { get; set; } = default!;
    public static RabbitMqSettings RabbitMqSettings { get; set; } = default!;

    public static void Initialize(Database database, RabbitMqSettings rabbitMqSettings)
    {
        Database = database;
        RabbitMqSettings = rabbitMqSettings;
    }
}