namespace DotNet10wConfigLog;

/// <summary>
/// Strongly-typed settings bound from the "App" section of appsettings.json.
/// </summary>
public sealed class AppSettings
{
    public const string SectionName = "AppSettings";

    public string Greeting { get; set; } = string.Empty;
}
