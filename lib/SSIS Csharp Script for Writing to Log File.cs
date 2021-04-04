#region Namespaces
using System;
using System.Data;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;
using System.Windows.Forms;
#endregion

namespace ST_56a150b321ab40bf8dd4e4dfeed00698
{
	[Microsoft.SqlServer.Dts.Tasks.ScriptTask.SSISScriptTaskEntryPointAttribute]
	public partial class ScriptMain : Microsoft.SqlServer.Dts.Tasks.ScriptTask.VSTARTScriptObjectModelBase
	{
		public void Main()
		{           
            //there is some non-obvious package logging config required to make this work
            //http://stackoverflow.com/questions/4348476/ssis-why-is-this-not-logging
            
            var consoleOutputStringBuilder = new StringBuilder();
		    consoleOutputStringBuilder.AppendLine();

		    //consoleOutputStringBuilder.AppendLine("Line One");
            //consoleOutputStringBuilder.AppendLine("Line Two");
            //consoleOutputStringBuilder.AppendLine("Line Three");
            //consoleOutputStringBuilder.AppendLine("The quick brown fox jumped over the lazy dog. The quick brown fox jumped over the lazy dog.  The quick brown fox jumped over the lazy dog.  The quick brown fox jumped over the lazy dog. LINE END");

		    consoleOutputStringBuilder.Append(Dts.Variables["User::ReportRecordCounterOutput"].Value);
            
            var consoleOutput = consoleOutputStringBuilder.ToString();

            var emptyBytes = new byte[0];

            Dts.Log("BEGIN REPORT RECORD COUNTER OUTPUT", 0, emptyBytes);
            Dts.Log(consoleOutput, 0, emptyBytes);
            Dts.Log("END REPORT RECORD COUNTER OUTPUT", 0, emptyBytes);

			Dts.TaskResult = (int)ScriptResults.Success;
		}

        #region ScriptResults declaration
        enum ScriptResults
        {
            Success = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success,
            Failure = Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure
        };
        #endregion

	}
}