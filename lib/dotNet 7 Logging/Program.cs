using ISEEImageToPdfConverter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

const string TimeStampFormat = "yyyy-mm-dd HH:mm:ss";
const string Tab = "   "; // actual tab char not supported log

IConfigurationRoot config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services
            .Configure<AppSettings>(config.GetSection("AppSettings"))
            .AddLogging(loggingBuilder => {
                var loggingSection = config.GetSection("Logging");
                loggingBuilder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = false;
                    options.SingleLine = true;
                    options.UseUtcTimestamp = false;
                    options.TimestampFormat = TimeStampFormat;
                });
                loggingBuilder.AddFile(loggingSection, fileLoggerOpts => {
                    fileLoggerOpts.FormatLogEntry = (msg) =>
                    {
                        var sb = new System.Text.StringBuilder();
                        sb.Append(DateTime.Now.ToString(TimeStampFormat));
                        sb.Append($"{Tab}{msg.LogLevel.ToString().PadRight("Information".Length)}{Tab}");
                        sb.Append(msg.Message);
                        if (msg.Exception != null)
                        {
                            sb.Append(Tab);
                            sb.Append(msg.Exception?.ToString());
                        }
                        return sb.ToString();
                    };
                });
            })
            .AddSingleton<ITiffToPdfConverterService, TiffToPdfConverterService>();
    })
    .Build();

using ILoggerFactory loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
ILogger<TiffToPdfConverterService> loggerHost = loggerFactory.CreateLogger<TiffToPdfConverterService>();
var log = new Log(loggerHost);

var converterService = host.Services.GetService(typeof(ITiffToPdfConverterService)) as ITiffToPdfConverterService;
var returnCode = converterService?.ConvertImages(log) ?? -100;

return returnCode;




