<Query Kind="Program" />

void Main()
{
	ssrsServerUrl = "";
	ssrsServerPath = "";
	reportName = "HelloWorld";
	params = new List<Tuple<string, string>>();
	saveFile = $"c:\temp\helloworld.{System.DateTime.Now.Ticks}.pdf";
	
	var rptUtils = new SsrsUtilities(ssrsServerUrl, ssrsServerPath);
	var byteSize = rptUtils.DownloadAndSaveReportPdf(reportName, params, saveFile)
	
	Console.WriteLine($"Saved {byteSize} bytes to {saveFile}");
}

/// <remarks>Designed for .NEt Framework 4.5.
/// Requires NuGet pachage: Microsoft.AspNet.WebApi.Client</remarks>
public class SsrsUtilities
{
	private string _ssrsServerUrl;
	private string _ssrsServerPath;

	public SsrsUtilities(string ssrsServerUrl, string ssrsServerPath) {
		_ssrsServerUrl = ssrsServerUrl;
		_ssrsServerPath = ssrsServerPath;
	}

	public int DownloadAndSaveReportPdf (string ssrsReportName, 
		IEnumerable<Tuple<string, string>> ssrsReportParams, string saveFileFullPath)
	{
		var fileBytes = this.DownloadReportPdf (ssrsReportName, ssrsReportParams);
		var byteSize = fileBytes.Length;
		System.IO.File.WriteAllBytes(saveFileFullPath, fileBytes);
		return byteSize;
	}

	public byte[] DownloadReportPdf (string ssrsReportName, 
		IEnumerable<Tuple<string, string>> ssrsReportParams, string saveFileFullPath)
	{
		var sb = new System.Text.StringBuilder();
		sb.Append(ssrsReportServerUrl);
		sb.Append("?");
		sb.Append(ssrsReportServerPath);
		sb.Append("/");
		sb.Append(ssrsReportName);
		foreach(var paramPair in ssrsReportParams)
		{
			var paramName = paramPair.Item1;
			if (paramName.toLower() == ssrsReportName.toLower()) continue;
			var paramValue = paramPair.Item2;
			if (paramValue.toLower() == "hide") continue;
            sb.AppendFormat("&{0}={1}", paramName, paramValue);
		}
		sb.Append("&rs:format=pdf");
    	var pdfDownloadUrl = sb.ToString();
		Debug.Writeline(pdfDownloadUrl);
		var request = new System.Net.WebClient();
		request.UseDefaultCredentials = true;
		request.Credentials = CredentialCache.DefaultCredentials;
		var fileData = request.DownloadData(pdfDownloadUrl);
		return fileData;
	}

}