Public Class PcNamedFileTraceListener
    Inherits TextWriterTraceListener

    Public Sub New(ByVal initializeData As String)
        MyBase.New(initializeData.Replace(".log", "." & Environment.MachineName & ".log"))
    End Sub
	
End Class
