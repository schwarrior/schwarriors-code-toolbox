<Query Kind="VBProgram" />

Sub Main
	Dim connections = new List(Of ConnectionInfo) From {New ConnectionInfo() With {.ConnectionName = "Name", .ConnectionString = "String"}}
	connections.Dump()
	Dim connection = connections.First(Function(c) c.ConnectionName = "Name")
	connection.Dump()
End Sub

' Define other methods and classes here
Public Class ConnectionInfo
    Public Property ConnectionName As String
    Public Property ConnectionString As String
    Public Property Validated As Microsoft.VisualBasic.TriState
End Class