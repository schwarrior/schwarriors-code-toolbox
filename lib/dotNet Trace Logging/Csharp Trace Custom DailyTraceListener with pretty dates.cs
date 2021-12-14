using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualBasic;

namespace SampleProgram
{
    public class DailyTraceListener : TraceListener
    {
        private readonly string _logFileLocation;
        private DateTime _currentDate;
        private StreamWriter _traceWriter;

        public DailyTraceListener(string fileName)
        {
            _logFileLocation = fileName;
            _traceWriter = new StreamWriter(GenerateFileName(), true);
        }

        public override void Write(string message)
        {
            CheckRollover();
            var fMessage = FormatMessage(message);
            _traceWriter.Write(fMessage);
        }

        public override void Write(string message, string category)
        {
            CheckRollover();
            var fMessage = FormatMessage(message, category);
            _traceWriter.Write(fMessage);
        }

        public override void WriteLine(string message)
        {
            CheckRollover();
            var fMessage = FormatMessage(message);
            _traceWriter.WriteLine(fMessage);
        }

        public override void WriteLine(string message, string category)
        {
            CheckRollover();
            var fMessage = FormatMessage(message, category);
            _traceWriter.WriteLine(fMessage);
        }

        private void FormatLogLine(string message, string category = "Information")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now);
            sb.Append(Constants.vbTab);
            sb.Append(Environment.MachineName);
            sb.Append(Constants.vbTab);
            sb.Append(category);
            sb.Append(Constants.vbTab);
            sb.Append(message);
            return sb.ToString();
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
                    _traceWriter.Flush();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _traceWriter.Close();
        }
    }
}
