using Microsoft.Extensions.Configuration;
using System.IO;

/// <summary>
/// For asp.net core projects
/// Reads setting keys from appsettings.json at the root level
/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration#simple-configuration
/// </summary>
public class ConfigurationProvider
{
    public static IConfigurationRoot Configuration { get; set; }

    public static string GetConfigurationValue(string key)
    {
        var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

        Configuration = builder.Build();

        return Configuration[key];
    }

    /// <summary>
    /// Reads connections strings from inside the ConnectionStrings section of the appsettings.json file
    /// </summary>
    /// <param name="connectionName"></param>
    /// <returns></returns>
    public static string GetDatabaseConnectionString(string connectionName)
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        var connectionStringConfig = builder.Build();

        return connectionStringConfig.GetConnectionString(connectionName);
    }
}
