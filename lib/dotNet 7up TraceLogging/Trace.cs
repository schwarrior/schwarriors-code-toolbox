
using Microsoft.Extensions.Logging;

public class Trace
{
	public const string logCategoryName = "Image Convert";

	private static ILoggerFactory? _loggerFactory = null;

	private static ILoggerFactory loggerFactory
	{
		get
		{
			if (_loggerFactory == null)
			{
				_loggerFactory = LoggerFactory.Create(builder =>
				builder.AddSimpleConsole(options =>
				{
					options.IncludeScopes = false;
					options.SingleLine = false;
					options.UseUtcTimestamp = false;
					options.TimestampFormat = "yyyy-mm-dd HH:mm:ss";
				}));
			}
			return _loggerFactory;
		}
	}

	private static ILogger? _logger = null;

	public static ILogger logger
	{
		get
		{
			if (_logger == null)
			{
				_logger = loggerFactory.CreateLogger(logCategoryName);
			}
			return _logger;
		}
	}

	public static void LogDebug(string message)
	{
		logger.LogDebug(message);
	}

	public static void LogInfo(string message)
	{
		logger.LogInformation(message);
	}

	public static void LogWarning(string message)
	{
		logger.LogWarning(message);
	}

	public static void LogError(string message)
	{
		logger.LogError(message);
	}

	public static void LogError(Exception ex)
	{
		logger.LogError(new EventId(ex.HResult, ex.Message), ex, null);
	}

	public static void LogError(Exception ex, string message)
	{
		var comboMsg = $"{message}. {ex}";
		logger.LogError(new EventId(ex.HResult, ex.Message), ex, comboMsg);
	}

	public static void LogCritical(string message)
	{
		logger.LogCritical(message);
	}

	public static void LogCritical(Exception ex)
	{
		logger.LogCritical(new EventId(ex.HResult, ex.Message), ex, null);
	}

	public static void LogCritical(Exception ex, string message)
	{
		var comboMsg = $"{message}. {ex}";
		logger.LogCritical(new EventId(ex.HResult, ex.Message), ex, comboMsg);
	}

}