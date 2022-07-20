using System.IO;
using System.Reflection;
using System.Text;

// this can be used for logging across projects
// known working pattern: TraceWriter class in referenced assembly. systemdiagnostics config entries in executing startup project

public class TraceWriter
{
	private static string _traceSourceName = "SampleProgram";

	private static TraceSource _traceSource;

	private static void Initialize()
	{
	    if (_traceSource == null)
		_traceSource = new TraceSource(_traceSourceName); // { Switch = { Level = SourceLevels.All } };
	}

	public static void TraceInfo(string info)
	{
	    Write(TraceEventType.Information, info);
	}

	public static void TraceInfo(string format, params object[] args)
	{
	    var info = string.Format(format, args);
	    Write(TraceEventType.Information, info);
	}

	public static void TraceWarning(string info)
	{
	    Write(TraceEventType.Warning, info);
	}

	public static void TraceWarning(string format, params object[] args)
	{
	    var info = string.Format(format, args);
	    Write(TraceEventType.Warning, info);
	}

	public static void TraceVerbose(string info)
	{
	    Write(TraceEventType.Verbose, info);
	}

	public static void TraceVerbose(string format, params object[] args)
	{
	    var info = string.Format(format, args);
	    Write(TraceEventType.Verbose, info);
	}

	public static void TraceError(Exception ex)
	{
	    var info = ex.ToString();
	    Write(TraceEventType.Error, info);
	}

	public static void TraceError(string message)
	{
	    Write(TraceEventType.Error, message);
	}

	public static void TraceError(string format, params object[] args)
	{
	    var info = string.Format(format, args);
	    Write(TraceEventType.Error, info);
	}


	private static void Write(TraceEventType type, string message)
	{
	    Initialize();
	    _traceSource.TraceEvent(type, 0, message);
	    _traceSource.Flush();
	}

	public static void TraceEnvironmentInfo()
	{
	    var app = Assembly.GetExecutingAssembly();

	    TraceInfo($"Program Name: {app.GetName().Name}");
	    TraceInfo($"Program Path: {app.Location}");
	    TraceInfo($"Program Version: {app.GetName().Version}");
	    var args = Environment.GetCommandLineArgs();
	    var argsOnly = new List<string>();
	    for (int i = 1; i < args.Length; i++)
	    {
		argsOnly.Add(args[i]);
	    }
	    var argsFlat = CollectionToFlattenedString(argsOnly);
	    TraceInfo($"Current Directory: {Environment.CurrentDirectory}");
	    TraceInfo($"Command Line Args: {argsFlat}");
	    TraceInfo($"Is 64 Bit Process: {Environment.Is64BitProcess}");
	    var logs = GetTextWriterOutputPaths();
	    var logsFlat = CollectionToFlattenedString(logs);
	    TraceInfo($"Logs to: {logsFlat}");
	    TraceInfo($"User Domain: {Environment.UserDomainName}");
	    TraceInfo($"User: {Environment.UserName}");
	    TraceInfo($"Interactive User: {Environment.UserInteractive}");
	    TraceInfo($"Machine: {Environment.MachineName}");
	    TraceInfo($"OS Version: {Environment.OSVersion}");
	    TraceInfo($"Processor Count: {Environment.ProcessorCount}");
		var dbConnStr = new MyDbContext().Database.Connection.ConnectionString;
		var cleanDbConnStr = CleanConnectionString(dbConnStr);
		TraceInfo($"DB Connection String: {cleanDbConnStr}");
	}

	public static string CleanConnectionString(string connStr)
	{
		var passwordSubstitute = "*******";
		var builder = new SqlConnectionStringBuilder(connStr);
		if (!string.IsNullOrWhiteSpace(builder.Password))
		{
			builder.Password = passwordSubstitute;
		}
		return builder.ToString();
	}

	public static IEnumerable<string> GetTextWriterOutputPaths()
	{
	    Initialize();
	    var logFileListenerType = typeof(TextWriterTraceListener);
	    foreach (var listener in _traceSource.Listeners)
	    {               
		var currentListenerType = listener.GetType();
		if (currentListenerType != logFileListenerType && !currentListenerType.IsSubclassOf(logFileListenerType)) continue;
		var fInfo = currentListenerType.GetProperty("Writer", BindingFlags.Public | BindingFlags.Instance);
		if (fInfo == null) continue;
		var writer = fInfo.GetValue(listener);
		if (writer.GetType() != typeof(StreamWriter)) continue;
		var streamWriter = writer as StreamWriter;
		var fileStream = streamWriter?.BaseStream as FileStream;
		yield return fileStream?.Name;
	    }
	}

	public static string CollectionToFlattenedString<T>(IEnumerable<T> collection, int characterDisplayLimit = int.MaxValue, int itemDisplayLimit = int.MaxValue, string noItemsString = "None")
	{
	    var list = new List<T>(collection); //enable indexing. prevent multiple enums

	    if (!list.Any()) return noItemsString;

	    var sb = new StringBuilder();
	    var listIndex = 0;
	    for (listIndex = 0; listIndex < list.Count() && listIndex < itemDisplayLimit; listIndex++)
	    {
		var itemString = list[listIndex].ToString();
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

	
class Program
{

	static void Main(string[] args)
	{
	    TraceWriter.TraceEnvironmentInfo();
	    TraceWriter.TraceVerbose("Test Trace Verbose");
	    TraceWriter.TraceVerbose("Test Trace Verbose with arg: {0}", "hello world");
	    TraceWriter.TraceInfo("Test Trace Info");
	    TraceWriter.TraceInfo("Test Trace Info with arg: {0}", "hello world");
	    TraceWriter.TraceWarning("Test Trace Warning");
	    TraceWriter.TraceWarning("Test Trace Warning with arg: {0}", "hello world");
	    TraceWriter.TraceError("Test Trace Error");
	    TraceWriter.TraceError("Test Trace Error with arg: {0}", "hello world");
	    TraceWriter.TraceError(new Exception("Test Exception"));
	    Console.ReadLine();
	}
	
}
