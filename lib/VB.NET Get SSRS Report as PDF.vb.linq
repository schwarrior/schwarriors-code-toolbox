<Query Kind="VBProgram" />

Sub Main
	Dim ssrsServerUrl = "http://SsrsServer1/ReportServer"
	Dim ssrsServerPath = "/ProjectA/Dev"
	Dim reportName = "HelloWorld"
	Dim parameters = New List(Of Tuple(Of String, String)) From {
		New Tuple(Of String, String)("Color", "Red")
	}
	Dim saveFile = $"C:\\Temp\\{reportName}.{System.DateTime.Now.Ticks}.pdf"
	Dim rptUtils = New SsrsUtilities(ssrsServerUrl, ssrsServerPath)
	Dim byteSize = rptUtils.DownloadAndSaveReportPdf(reportName, parameters, saveFile)
	Console.WriteLine($"Saved {byteSize} bytes to {saveFile}")
End Sub

Public Class SsrsUtilities
	Private _ssrsServerUrl As String
	Private _ssrsServerPath As String

	Public Sub New(ByVal ssrsServerUrl As String, ByVal ssrsServerPath As String)
		_ssrsServerUrl = ssrsServerUrl
		_ssrsServerPath = ssrsServerPath
	End Sub

	Public Function DownloadAndSaveReportPdf(ByVal ssrsReportName As String, ByVal ssrsReportParams As IEnumerable(Of Tuple(Of String, String)), ByVal saveFileFullPath As String) As Integer
		Dim fileBytes = Me.DownloadReportPdf(ssrsReportName, ssrsReportParams)
		Dim byteSize = fileBytes.Length
		System.IO.File.WriteAllBytes(saveFileFullPath, fileBytes)
		Return byteSize
	End Function

	Public Function DownloadReportPdf(ByVal ssrsReportName As String, ByVal ssrsReportParams As IEnumerable(Of Tuple(Of String, String))) As Byte()
		Dim sb = New System.Text.StringBuilder()
		sb.Append(Me._ssrsServerUrl)
		If Not sb.ToString().EndsWith("/") Then sb.Append("/")
		sb.Append("Pages/ReportViewer.aspx?")
		If Not Me._ssrsServerPath.StartsWith("/") Then sb.Append("/")
		sb.Append(Me._ssrsServerPath)
		If Not sb.ToString().EndsWith("/") Then sb.Append("/")
		sb.Append(ssrsReportName)

		For Each paramPair In ssrsReportParams
			Dim paramName = paramPair.Item1
			If paramName.ToLower() = ssrsReportName.ToLower() Then Continue For
			Dim paramValue = paramPair.Item2
			If paramValue.ToLower() = "hide" Then Continue For
			sb.AppendFormat("&{0}={1}", paramName, paramValue)
		Next

		sb.Append("&rs:format=pdf")
		Dim pdfDownloadUrl = sb.ToString()
		Debug.WriteLine(pdfDownloadUrl)
		Dim request = New System.Net.WebClient()
		request.UseDefaultCredentials = True
		request.Credentials = System.Net.CredentialCache.DefaultCredentials
		Dim fileData = request.DownloadData(pdfDownloadUrl)
		Return fileData
	End Function
End Class
