using System;
using System.Diagnostics;

namespace DotNetDatedLogFileLab
{
    public class NewFileTraceListener : TextWriterTraceListener
    {
        public NewFileTraceListener(string initializeData) 
            : base(initializeData.Replace(".log", "."+DateTime.Now.Ticks.ToString()+".log"))
        {
        }

    }
}
