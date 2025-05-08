using CrossCutting.Settings.Models;

namespace CrossCutting.Settings;

public static class AppSettings
{
    public static Database Database { get; set; } = default!;

    public static void Initialize(Database database)
    {
        Database = database;
    }
}