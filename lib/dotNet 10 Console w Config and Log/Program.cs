using dotNet_10_Console_w_Config_and_Log;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// CreateApplicationBuilder wires up:
//   - Configuration: appsettings.json, appsettings.{Environment}.json, environment variables, command-line args
//   - Logging: console/debug providers, filtered by the "Logging" section of configuration
var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppSettings.SectionName));
builder.Services.AddTransient<Worker>();

using var host = builder.Build();

// Ctrl+C cancels the token so Worker can shut down gracefully.
using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

var worker = host.Services.GetRequiredService<Worker>();
return await worker.RunAsync(args, cts.Token);
