Imports System.IO
Imports System.Reflection
Imports System.Text

Public Class TraceWriter
    Private Shared _traceSourceName As String = "SampleProgram"
    Private Shared _traceSource As TraceSource

    Private Shared Sub Initialize()
        If _traceSource Is Nothing Then _traceSource = New TraceSource(_traceSourceName)
    End Sub

    Public Shared Sub TraceInfo(ByVal info As String)
        Write(TraceEventType.Information, info)
    End Sub

    Public Shared Sub TraceInfo(ByVal format As String, ParamArray args As Object())
        Dim info = String.Format(format, args)
        Write(TraceEventType.Information, info)
    End Sub

    Public Shared Sub TraceWarning(ByVal info As String)
        Write(TraceEventType.Warning, info)
    End Sub

    Public Shared Sub TraceWarning(ByVal format As String, ParamArray args As Object())
        Dim info = String.Format(format, args)
        Write(TraceEventType.Warning, info)
    End Sub

    Public Shared Sub TraceVerbose(ByVal info As String)
        Write(TraceEventType.Verbose, info)
    End Sub

    Public Shared Sub TraceVerbose(ByVal format As String, ParamArray args As Object())
        Dim info = String.Format(format, args)
        Write(TraceEventType.Verbose, info)
    End Sub

    Public Shared Sub TraceError(ByVal ex As Exception)
        Dim info = ex.ToString()
        Write(TraceEventType.[Error], info)
    End Sub

    Public Shared Sub TraceError(ByVal message As String)
        Write(TraceEventType.[Error], message)
    End Sub

    Public Shared Sub TraceError(ByVal format As String, ParamArray args As Object())
        Dim info = String.Format(format, args)
        Write(TraceEventType.[Error], info)
    End Sub

    Private Shared Sub Write(ByVal type As TraceEventType, ByVal message As String)
        Initialize()
        _traceSource.TraceEvent(type, 0, message)
        _traceSource.Flush()
    End Sub

    Public Shared Sub TraceEnvironmentInfo()
        Dim app = Assembly.GetExecutingAssembly()
        TraceInfo($"Program Name: {app.GetName().Name}")
        TraceInfo($"Program Path: {app.Location}")
        TraceInfo($"Program Version: {app.GetName().Version}")
        Dim args = Environment.GetCommandLineArgs()
        Dim argsOnly = New List(Of String)()

        For i As Integer = 1 To args.Length - 1
            argsOnly.Add(args(i))
        Next

        Dim argsFlat = CollectionToFlattenedString(argsOnly)
        TraceInfo($"Current Directory: {Environment.CurrentDirectory}")
        TraceInfo($"Command Line Args: {argsFlat}")
        TraceInfo($"Is 64 Bit Process: {Environment.Is64BitProcess}")
        Dim logs = GetTextWriterOutputPaths()
        Dim logsFlat = CollectionToFlattenedString(logs)
        TraceInfo($"Logs to: {logsFlat}")
        TraceInfo($"User Domain: {Environment.UserDomainName}")
        TraceInfo($"User: {Environment.UserName}")
        TraceInfo($"Interactive User: {Environment.UserInteractive}")
        TraceInfo($"Machine: {Environment.MachineName}")
        TraceInfo($"OS Version: {Environment.OSVersion}")
        TraceInfo($"Processor Count: {Environment.ProcessorCount}")
		Dim dbConnStr = New MyDbContext().Database.Connection.ConnectionString
        Dim cleanDbConnStr = CleanConnectionString(dbConnStr)
        TraceInfo($"DB Connection String: {cleanDbConnStr}")
    End Sub

    Public Shared Iterator Function GetTextWriterOutputPaths() As IEnumerable(Of String)
        Initialize()
        Dim logFileListenerType = GetType(TextWriterTraceListener)

        For Each listener In _traceSource.Listeners
            Dim currentListenerType = listener.[GetType]()
            If currentListenerType <> logFileListenerType AndAlso Not currentListenerType.IsSubclassOf(logFileListenerType) Then Continue For
            Dim fInfo = currentListenerType.GetProperty("Writer", BindingFlags.[Public] Or BindingFlags.Instance)
            If fInfo Is Nothing Then Continue For
            Dim writer = fInfo.GetValue(listener)
            If writer.[GetType]() <> GetType(StreamWriter) Then Continue For
            Dim streamWriter = TryCast(writer, StreamWriter)
            Dim fileStream = TryCast(streamWriter?.BaseStream, FileStream)
            Yield fileStream?.Name
        Next
    End Function

    Public Shared Function CollectionToFlattenedString(Of T)(ByVal collection As IEnumerable(Of T), ByVal Optional characterDisplayLimit As Integer = Integer.MaxValue, ByVal Optional itemDisplayLimit As Integer = Integer.MaxValue, ByVal Optional noItemsString As String = "None") As String
        Dim list = New List(Of T)(collection)
        If Not list.Any() Then Return noItemsString
        Dim sb = New StringBuilder()
        Dim listIndex = 0
        listIndex = 0

        While listIndex < list.Count() AndAlso listIndex < itemDisplayLimit
            Dim itemString = list(listIndex).ToString()

            If Not String.IsNullOrEmpty(itemString) Then

                If sb.Length > 0 Then
                    sb.Append(", ")
                End If

                sb.Append(itemString)
            End If

            listIndex += 1
        End While

        Dim remaining = list.Count - (listIndex + 1)

        If remaining > 0 Then
            sb.AppendFormat(" and {0} more ...", remaining)
        End If

        Return StringToEllipsisLimitedString(sb.ToString(), characterDisplayLimit)
    End Function

    Public Shared Function StringToEllipsisLimitedString(ByVal fullString As String, ByVal characterLimit As Integer, ByVal Optional ellipsisText As String = "...") As String
        If fullString.Length > characterLimit Then Return fullString.Substring(0, characterLimit - ellipsisText.Length) & ellipsisText
        Return fullString
    End Function

	Public Shared Function CleanConnectionString(ByVal connStr As String) As String
        Dim passwordSubstitute = "*******"
        Dim builder = New SqlConnectionStringBuilder(connStr)

        If Not String.IsNullOrWhiteSpace(builder.Password) Then
            builder.Password = passwordSubstitute
        End If

        Return builder.ToString()
    End Function

End Class

Class Program
    Private Shared Sub Main(ByVal args As String())
        TraceWriter.TraceEnvironmentInfo()
        TraceWriter.TraceVerbose("Test Trace Verbose")
        TraceWriter.TraceVerbose("Test Trace Verbose with arg: {0}", "hello world")
        TraceWriter.TraceInfo("Test Trace Info")
        TraceWriter.TraceInfo("Test Trace Info with arg: {0}", "hello world")
        TraceWriter.TraceWarning("Test Trace Warning")
        TraceWriter.TraceWarning("Test Trace Warning with arg: {0}", "hello world")
        TraceWriter.TraceError("Test Trace Error")
        TraceWriter.TraceError("Test Trace Error with arg: {0}", "hello world")
        TraceWriter.TraceError(New Exception("Test Exception"))
        Console.ReadLine()
    End Sub
End Class
