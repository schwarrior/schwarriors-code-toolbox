public class Program
{
	public static int Main()
	{
		Trace.LogInfo("LogLab7 Starting");

		Trace.LogDebug("Mock Debug Info");
		Trace.LogWarning("Mock Warning");
		Trace.LogError(new Exception("Mock Exception"));
		Trace.LogError(new Exception("Mock Exception"), "An Additional Error Comment");
		Trace.LogCritical(new Exception("Mock Critical Exception"));
		Trace.LogCritical(new Exception("Mock Exception"), "An Additional Critical Comment");

		Trace.LogInfo("LogLab7 Done");

		Console.WriteLine();
		Console.WriteLine("Press Enter to Exit");
		Console.ReadLine();
		return 0;
	}
}