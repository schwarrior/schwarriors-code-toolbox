<Query Kind="Program" />

void Main()
{
	var ssrsServerUrl = "http://SsrsServer1/ReportServer";
	var ssrsServerPath = "ProjectA/Dev";
	var reportName = "HelloWorld";
	var parameters = new List<Tuple<string,string>> {
		new Tuple<string, string>("Color", "Red")
	};
	
	var saveFile = $"C:\\Temp\\{reportName}.{System.DateTime.Now.Ticks}.pdf"
	
	var rptUtils = new SsrsUtilities(ssrsServerUrl, ssrsServerPath);
	var byteSize = rptUtils.DownloadAndSaveReportPdf(reportName, parameters, saveFile);
	
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

	public byte[] DownloadReportPdf (string ssrsReportName, IEnumerable<Tuple<string, string>> ssrsReportParams)
	{
		var sb = new System.Text.StringBuilder();
		sb.Append(this._ssrsServerUrl);
		if (!sb.ToString().EndsWith("/")) sb.Append("/");
		sb.Append("Pages/ReportViewer.aspx?");
		if (!this._ssrsServerPath.StartsWith("/")) sb.Append("/");
		sb.Append(this._ssrsServerPath);
		if (!sb.ToString().EndsWith("/")) sb.Append("/");
		sb.Append(ssrsReportName);
		foreach(var paramPair in ssrsReportParams)
		{
			var paramName = paramPair.Item1;
			if (paramName.ToLower() == ssrsReportName.ToLower()) continue;
			var paramValue = paramPair.Item2;
			if (paramValue.ToLower() == "hide") continue;
            sb.AppendFormat("&{0}={1}", paramName, paramValue);
		}
		sb.Append("&rs:format=pdf");
    	var pdfDownloadUrl = sb.ToString();
		Debug.WriteLine(pdfDownloadUrl);
		var request = new System.Net.WebClient();
		request.UseDefaultCredentials = true;
		request.Credentials = System.Net.CredentialCache.DefaultCredentials;
		var fileData = request.DownloadData(pdfDownloadUrl);
		return fileData;
	}

}