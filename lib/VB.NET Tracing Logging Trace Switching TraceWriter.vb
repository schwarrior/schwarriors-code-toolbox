Imports System
Imports System.Diagnostics
Imports System.Reflection

Public Class TraceWriter
    Private Shared _traceSource As TraceSource

    Private Shared Sub Initialize()
        If _traceSource Is Nothing Then _traceSource = New TraceSource("SampleProgram")
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

    Public Shared Function GetTextWriterFilePath() As String
        Initialize()

        For Each listener In _traceSource.Listeners
            Dim textLogWriter = TryCast(listener, TextWriterTraceListener)
            If textLogWriter Is Nothing OrElse listener.[GetType]() <> GetType(TextWriterTraceListener) Then Continue For
            Dim fInfo = textLogWriter.[GetType]().GetField("initializeData", BindingFlags.NonPublic Or BindingFlags.Instance)
            If fInfo Is Nothing Then Continue For
            Dim filePath = CStr(fInfo.GetValue(textLogWriter))
            Return filePath
        Next

        Return String.Empty
    End Function
End Class

Class Program
    Private Shared Sub Main(ByVal args As String())
        TraceWriter.TraceVerbose("Test Trace Verbose")
        TraceWriter.TraceVerbose("Test Trace Verbose with arg: {0}", "hello world")
        TraceWriter.TraceInfo("Test Trace Info")
        TraceWriter.TraceInfo("Test Trace Info with arg: {0}", "hello world")
        TraceWriter.TraceWarning("Test Trace Warning")
        TraceWriter.TraceWarning("Test Trace Warning with arg: {0}", "hello world")
        TraceWriter.TraceError("Test Trace Error")
        TraceWriter.TraceError("Test Trace Error with arg: {0}", "hello world")
        TraceWriter.TraceError(New Exception("Test Exception"))
    End Sub
End Class
