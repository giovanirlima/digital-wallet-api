namespace CrossCutting.Settings.Models;

public class Database
{
    public string ReadHost { get; set; } = default!;
    public string WriteHost { get; set; } = default!;
    public string Base { get; set; } = default!;
    public string User { get; set; } = default!;
    public string Password { get; set; } = default!;
}