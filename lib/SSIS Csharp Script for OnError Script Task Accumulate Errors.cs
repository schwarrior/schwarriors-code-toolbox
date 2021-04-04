
#region Namespaces
using System;
using System.Data;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;
using System.Windows.Forms;
#endregion

namespace ST_b0939bc0ffc24c86a46c62165cbbe1f1
{
	[Microsoft.SqlServer.Dts.Tasks.ScriptTask.SSISScriptTaskEntryPointAttribute]
	public partial class ScriptMain : Microsoft.SqlServer.Dts.Tasks.ScriptTask.VSTARTScriptObjectModelBase
	{
        
		public void Main()
		{
            var packageName = Dts.Variables["System::PackageName"].Value.ToString();
            var machineName = Dts.Variables["System::MachineName"].Value.ToString();
		    Dts.Variables["User::ErrorAlertEmailSubject"].Value =
		        string.Format("Errors have occured in the package '{0}' running on machine '{1}'", packageName,
		            machineName).Replace('"','\'');
            var sb = new StringBuilder();
            var existingErrorMessage = Dts.Variables["User::ErrorAlertEmailBody"].Value.ToString();
		    if (!string.IsNullOrEmpty(existingErrorMessage)) sb.AppendLine(existingErrorMessage);
		    var task = Dts.Variables["System::SourceName"].Value.ToString();
            var error = Dts.Variables["System::ErrorDescription"].Value.ToString();
            sb.AppendLine(string.Format("Task: '{0}'. Error: {1}", task, error));
		    Dts.Variables["User::ErrorAlertEmailBody"].Value = sb.ToString().Replace('"','\'');
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