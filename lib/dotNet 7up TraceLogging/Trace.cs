using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

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
                        options.SingleLine = true;
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

        public static void TraceEnvironmentInfo()
        {
            var app = Assembly.GetExecutingAssembly();

            LogInfo($"Program Name: {app.GetName().Name}");
            LogInfo($"Program Path: {app.Location}");
            LogInfo($"Program Version: {app.GetName().Version}");
            var args = Environment.GetCommandLineArgs();
            var argsOnly = new List<string>();
            for (int i = 1; i < args.Length; i++)
            {
                argsOnly.Add(args[i]);
            }
            var argsFlat = CollectionToFlattenedString(argsOnly);
            LogInfo($"Current Directory: {Environment.CurrentDirectory}");
            LogInfo($"Command Line Args: {argsFlat}");
            LogInfo($"Is 64 Bit Process: {Environment.Is64BitProcess}");
            LogInfo($"User Domain: {Environment.UserDomainName}");
            LogInfo($"User: {Environment.UserName}");
            LogInfo($"Interactive User: {Environment.UserInteractive}");
            LogInfo($"Machine: {Environment.MachineName}");
            LogInfo($"OS Version: {Environment.OSVersion}");
            LogInfo($"Processor Count: {Environment.ProcessorCount}");

            var configBuilder = new ConfigurationBuilder().
            AddJsonFile("appsettings.json").Build();
            var configSection = configBuilder.GetSection("AppSettings");
            var dbConnStr = configSection["connectionString"] ?? String.Empty;
            var cleanDbConnStr = CleanConnectionString(dbConnStr);
            LogInfo($"DB Connection String: {cleanDbConnStr}");

            var query = configSection["query"] ?? String.Empty;
            LogInfo($"Image Queue Query: {query}");

            var logFolder = configSection["logFolder"] ?? String.Empty;
            LogInfo($"Log Folder (Depricated): {logFolder}");

        }

        public static string CleanConnectionString(string connStr)
        {
			if ( String.IsNullOrWhiteSpace(connStr) ) { return String.Empty; } 
            var passwordSubstitute = "*******";
            var builder = new SqlConnectionStringBuilder(connStr);
            if ( !string.IsNullOrWhiteSpace(builder.Password) )
            {
                builder.Password = passwordSubstitute;
            }
            return builder.ToString();
        }

        public static string CollectionToFlattenedString<T>(IEnumerable<T> collection, int characterDisplayLimit = int.MaxValue, int itemDisplayLimit = int.MaxValue, string noItemsString = "None")
        {
            var list = new List<T>(collection); //enable indexing. prevent multiple enums

            if (!list.Any()) return noItemsString;

            var sb = new StringBuilder();
            var listIndex = 0;
            for (listIndex = 0; listIndex < list.Count() && listIndex < itemDisplayLimit; listIndex++)
            {
                var itemString = list[listIndex]!.ToString();
                if (!string.IsNullOrEmpty(itemString))
                {
                    if (sb.Length > 0) { sb.Append(", "); }
                    sb.Append(itemString);
                }
            }

            //if there are more items than the itemDisplayLimit, show "and X more ..."
            var remaining = list.Count - (listIndex + 1);
            if (remaining > 0)
            {
                sb.AppendFormat(" and {0} more ...", remaining);
            }

            return StringToEllipsisLimitedString(sb.ToString(), characterDisplayLimit);
        }

        public static string StringToEllipsisLimitedString(string fullString, int characterLimit, string ellipsisText = "...")
        {
            if (fullString.Length > characterLimit)
                return fullString.Substring(0, characterLimit - ellipsisText.Length) + ellipsisText;
            return fullString;
        }

    }
