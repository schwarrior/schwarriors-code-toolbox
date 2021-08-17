using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DotNetDatedLogFileLab
{
    public class DailyTraceListenerGen1 : TraceListener
    {
        private readonly string _logFileLocation;
        private DateTime _currentDate;
        StreamWriter _traceWriter;

        public DailyTraceListenerGen1(string fileName)
        {
            _logFileLocation = fileName;
            _traceWriter = new StreamWriter(GenerateFileName(), true);
        }

        public override void Write(string message)
        {
            CheckRollover();
            _traceWriter.Write(message);
        }

        public override void Write(string message, string category)
        {
            CheckRollover();
            _traceWriter.Write(category + " " + message);
        }


        public override void WriteLine(string message)
        {
            CheckRollover();
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now);
            sb.Append(": ");
            sb.Append(message);
            _traceWriter.WriteLine(sb.ToString());
        }

        public override void WriteLine(string message, string category)
        {
            WriteLine(message);
        }

        private string GenerateFileName()
        {
            _currentDate = DateTime.Today;
            return Path.Combine(Path.GetDirectoryName(_logFileLocation), Path.GetFileNameWithoutExtension(_logFileLocation) + "_" + _currentDate.ToString("yyyyMMdd") + Path.GetExtension(_logFileLocation));
        }

        private void CheckRollover()
        {
            if (_currentDate.CompareTo(DateTime.Today) != 0)
            {
                _traceWriter.Close();
                _traceWriter = new StreamWriter(GenerateFileName(), true);
            }
        }

        public override void Flush()
        {
            lock (this)
            {
                if (_traceWriter != null)
                {
                    _traceWriter.Flush();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _traceWriter.Close();
            }
        }
    }
}
