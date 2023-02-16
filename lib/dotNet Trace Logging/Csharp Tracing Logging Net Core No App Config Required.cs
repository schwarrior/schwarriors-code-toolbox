using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

//This can be used across projects
//known working pattern: TraceWriter class in referenced assembly. systemdiagnostics config entries in executing startup project

//todo test techniques in following article for reading settings from appsettings.json
//https://www.codeproject.com/Articles/5255953/Use-Trace-and-TraceSource-in-NET-Core-Logging

namespace SampleProgram.Library
{
    public class Log
    {
        private static object _lockTracker = new object();

        private static Stream _logFile = null; //possbible future enhancement - set back to null in class destructor

        private static TextWriterTraceListener _fileListener = null;

        private static string _assemblyName = string.Empty;

        private static void Initialize()
        {
            lock (_lockTracker)
            {
                if (_logFile == null)
                {
                    _assemblyName = Assembly.GetExecutingAssembly().GetName().Name; 
                    _assemblyName = _assemblyName.Split('.')[0]; //eg SampleProgram.Library becomes just SampleProgram

                    //Tracing to console seems to be automatic
                    //no need to add the a console listener

                    var logName = _assemblyName + ".log";
                    var logFullPath = Path.GetFullPath(logName); //path of executing assembly
                    var logInitMsg = $"Logging appends to existing file {logFullPath}";

                    if (File.Exists(logFullPath))
                    {
                        _logFile = new FileStream(logFullPath, FileMode.Append);
                    }
                    else
                    {
                        _logFile = new FileStream(logFullPath, FileMode.Create);
                        logInitMsg = $"Logging to new file {logFullPath}";
                    }
                    
                    _fileListener = new
                       TextWriterTraceListener(_logFile);
                    Trace.Listeners.Add(_fileListener);
                    LogInfo(logInitMsg);
                }
            }
        }

        public static void LogInfo(string message)
        {
            Initialize();
            Trace.TraceInformation($"{DateTime.Now.ToString("o")} - {message}");
            Trace.Flush();
        }

        public static void LogWarning(string message)
        {
            Initialize();
            Trace.TraceWarning($"{DateTime.Now.ToString("o")} - {message}");
            Trace.Flush();
        }

        public static void LogError(string message)
        {
            Initialize();
            Trace.TraceError($"{DateTime.Now.ToString("o")} - {message}");
            Trace.Flush();
        }

        public static void LogError(Exception ex)
        {
            Initialize();
            Trace.TraceError($"{DateTime.Now.ToString("o")} - {ex.ToString()}");
            Trace.Flush();
        }

		public static void LogError(string message, Exception ex)
		{
			Initialize();
			var comboMsg = $"{message}. {ex}";
			Trace.TraceError($"{DateTime.Now.ToString("o")} - {comboMsg}");
			Trace.Flush();
		}

    }
}
