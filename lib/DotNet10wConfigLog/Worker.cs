using Microsoft.Extensions.Options;
using System.Reflection;

namespace DotNet10wConfigLog;

/// <summary>
/// Application entry point. Put application code in <see cref="RunAsync"/>.
/// Add further dependencies to the constructor and register them in Program.cs.
/// </summary>
public sealed class Worker(
    ILogger<Worker> _logger,
    IOptions<AppSettings> options)
{
    private readonly AppSettings _settings = options.Value;

    public async Task<int> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        var resultCode = 0;

        var assembly = Assembly.GetEntryAssembly();
        var assemblyName = assembly?.GetName()?.Name ?? "App";
        var assemblyVersion = assembly?.GetName()?.Version?.ToString() ?? "0.0.0.0";

        _logger.LogInformation("Starting {prog} {ver}", assemblyName, assemblyVersion);

        _logger.LogInformation("Starting with console parameters: {args}", args);

        _logger.LogInformation("Greeting: {g}", _settings.Greeting);

        try
        {
            // to do: implement business logic here

            _logger?.LogInformation("{prog} completed", assemblyName);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            resultCode = 1;
        }

        await Task.CompletedTask;
        _logger?.LogInformation("{prog} exiting", assemblyName);
        return resultCode;
    }
}
