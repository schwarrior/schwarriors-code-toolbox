Imports System.Text

Namespace SampleProgram

	Public Class MachineNamedFileTraceListener
		Inherits TextWriterTraceListener

		Public Sub New(ByVal initializeData As String)
			MyBase.New(initializeData.Replace(".log", "." & Environment.MachineName & ".log"))
		End Sub

		Public Overrides Sub Write(ByVal message As String)
			Dim fMessage = FormatMessage(message)
			_traceWriter.Write(fMessage)
		End Sub

		Public Overrides Sub Write(ByVal message As String, ByVal category As String)
			Dim fMessage = FormatMessage(message, category)
			_traceWriter.Write(fMessage)
		End Sub

		Public Overrides Sub WriteLine(ByVal message As String)
			Dim fMessage = FormatMessage(message)
			_traceWriter.WriteLine(fMessage)
		End Sub

		Public Overrides Sub WriteLine(ByVal message As String, ByVal category As String)
			Dim fMessage = FormatMessage(message, category)
			_traceWriter.WriteLine(fMessage)
		End Sub

		Private Function FormatLogLine(ByVal message As String, ByVal category As String = "Information")
			Dim sb As StringBuilder = New StringBuilder()
			sb.Append(DateTime.Now)
			sb.Append(vbTab)
			sb.Append(Environment.MachineName)
			sb.Append(vbTab)
			sb.Append(category)
			sb.Append(vbTab)
			sb.Append(message)
			Return sb.ToString()
		End Function

	End Class

' SAMPLE CONFIG FILE SETUP

' <system.diagnostics>
' 	<sharedListeners>
' 		<add name="machineNamedFileListener" type="SampleProgram.MachineNamedFileListener, SampleProgram"  initializeData="c:\temp\SampleProgram.log" />
' 	</sharedListeners>
' 	<sources>
' 		<source name="SampleProgram" switchValue="Information" >
' 		<listeners>
' 			<remove name="Default" />
' 			<add name="machineNamedFileListener" />
' 		</listeners>
' 		</source>
' 	</sources>
' </system.diagnostics>

End Namespace
