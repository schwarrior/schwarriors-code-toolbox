Public Class NewFileTraceListener
    Inherits TextWriterTraceListener

    Public Sub New(ByVal initializeData As String)
        MyBase.New(initializeData.Replace(".log", "." & Environment.MachineName.ToString() & ".log"))
    End Sub

End Class
