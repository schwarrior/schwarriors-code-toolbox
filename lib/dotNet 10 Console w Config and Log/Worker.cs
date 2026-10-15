using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dotNet_10_Console_w_Config_and_Log;

/// <summary>
/// Application entry point. Put application code in <see cref="RunAsync"/>.
/// Add further dependencies to the constructor and register them in Program.cs.
/// </summary>
public sealed class Worker(
    ILogger<Worker> logger,
    IOptions<AppSettings> options,
    IConfiguration configuration)
{
    private readonly AppSettings _settings = options.Value;

    public async Task<int> RunAsync(string[] args, CancellationToken cancellationToken)
    {
        logger.LogInformation("Worker starting with {ArgCount} argument(s)", args.Length);

        // Strongly-typed config
        logger.LogInformation("Greeting: {Greeting}", _settings.Greeting);

        // Raw config access
        logger.LogDebug("Default log level: {Level}", configuration["Logging:LogLevel:Default"]);

        // TODO: application code goes here
        await Task.CompletedTask;

        logger.LogInformation("Worker finished");
        return 0;
    }
}
