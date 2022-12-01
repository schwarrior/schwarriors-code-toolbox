<Query Kind="VBProgram" />

Sub Main
	Dim p = "c:\temp\ad\lmu-answerdoc.pdf"
	Console.WriteLine(p)
	Dim s = GetSummaryFilePath(p)
	Console.WriteLine(s)
	Dim p2 = "lmu-answerdoc.pdf"
	Console.WriteLine(p2)
	Dim s2 = GetSummaryFilePath(p2)
	Console.WriteLine(s2)
End Sub

' Define other methods and classes here
Public Shared Function GetSummaryFilePath(fullFilePath As String) As String
	Dim fileName = Path.GetFileName(fullFilePath)
	Dim parentDirs = Path.GetDirectoryName(fullFilePath).Split(Path.DirectorySeparatorChar)
	If Not parentDirs.Any Then Return fileName
	Dim parentDir = parentDirs.Last
	Dim summaryPath = Path.Combine(parentDir, fileName)
	Return summaryPath
End Function