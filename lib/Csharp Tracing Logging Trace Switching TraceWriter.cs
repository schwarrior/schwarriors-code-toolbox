using System;
using System.Diagnostics;
using System.Reflection;

//This can be used across projects
//known working pattern: TraceWriter class in referenced assembly. systemdiagnostics config entries in executing startup project

public class TraceWriter
    {
        private static TraceSource _traceSource;

        private static void Initialize()
        {
            if (_traceSource == null)
                _traceSource = new TraceSource("SampleProgram"); // { Switch = { Level = SourceLevels.All } };
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

        public static string GetTextWriterFilePath()
        {
            Initialize();
            foreach (var listener in _traceSource.Listeners)
            {
                var textLogWriter = listener as TextWriterTraceListener;
                if (textLogWriter == null || listener.GetType() != typeof(TextWriterTraceListener)) continue;
                var fInfo = textLogWriter.GetType().GetField("initializeData", BindingFlags.NonPublic | BindingFlags.Instance); 
                if (fInfo == null) continue;
                var filePath = (string)fInfo.GetValue(textLogWriter);
                return filePath;
            }
            return string.Empty;
        }
    }
	
	
	class Program
    {

        static void Main(string[] args)
        {
			TraceWriter.TraceVerbose("Test Trace Verbose");
			TraceWriter.TraceVerbose("Test Trace Verbose with arg: {0}", "hello world");
            TraceWriter.TraceInfo("Test Trace Info");
			TraceWriter.TraceInfo("Test Trace Info with arg: {0}", "hello world");
			TraceWriter.TraceWarning("Test Trace Warning");
			TraceWriter.TraceWarning("Test Trace Warning with arg: {0}", "hello world");
			TraceWriter.TraceError("Test Trace Error");
			TraceWriter.TraceError("Test Trace Error with arg: {0}", "hello world");
			TraceWriter.TraceError(new Exception("Test Exception"));
		}
	}