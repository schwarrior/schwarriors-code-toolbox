Imports System.Text

Public Class MachineNamedFileTraceListener
    Inherits TextWriterTraceListener

    Public Sub New(ByVal initializeData As String)
        MyBase.New(initializeData.Replace(".log", "." & Environment.MachineName & ".log"))
    End Sub

    ''' <summary>
    ''' For each traced event, the trace source first calls write with prefixing meta data only
    ''' Then the trace source calls writeline with the message body only, which appends onto the previous write content
    ''' So by hooking write and not writeline, we are able to change the the prefixing metadata on each event
    ''' </summary>
    ''' <param name="message"></param>
    Public Overrides Sub Write(ByVal message As String)
        'Discard the second prefix element: typically the event code
        'to do: enhance this method to discard the UTC date meta data which might also be provided if configured in the .config
        Dim colonSplit = message.Split(CType(":", Char()))
        Dim fMessage = FormatMessage(colonSplit(0))
        MyBase.Write(fMessage)
    End Sub

    Private Function FormatMessage(ByVal message As String) As String
        Dim sb As StringBuilder = New StringBuilder()
        sb.Append(DateTime.Now)
        sb.Append(vbTab)
        sb.Append(Environment.MachineName)
        sb.Append(vbTab)
        sb.Append(message)
        sb.Append(vbTab)
        Return sb.ToString()
    End Function

End Class

' SAMPLE CONFIG FILE SETUP

'   <system.diagnostics>
'     <sharedListeners>
' 	  <add name="machineNamedFileTraceListener" type="SampleProgram.MachineNamedFileTraceListener, SampleProgram"  initializeData="c:\temp\SampleProgram.log" />
'     </sharedListeners>
'     <sources>
'       <source name="SampleProgram" switchValue="Information" >
'         <listeners>
'           <remove name="Default" />
' 		  <add name="machineNamedFileTraceListener" />
'         </listeners>
'       </source>
'     </sources>
'   </system.diagnostics>
