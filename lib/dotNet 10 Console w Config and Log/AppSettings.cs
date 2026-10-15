namespace dotNet_10_Console_w_Config_and_Log;

/// <summary>
/// Strongly-typed settings bound from the "App" section of appsettings.json.
/// </summary>
public sealed class AppSettings
{
    public const string SectionName = "App";

    public string Greeting { get; set; } = string.Empty;
}
