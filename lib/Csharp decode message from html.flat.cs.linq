<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.dll</Reference>
</Query>

void Main()
{
	// The flat version of this code file
	// takes all the methods and combines into one method
	// this is for easy porting to PowerShell
	
	var stream = new MemoryStream();
	var streamWriter = new StreamWriter(stream);
	streamWriter.Write(SampleHtml);
	streamWriter.Flush();
	stream.Position = 0;
	
	var errorMessage = GetErrorMessageFromSsrsHttpResponse(stream);
	Console.WriteLine(errorMessage);
}

string GetErrorMessageFromSsrsHttpResponse(Stream stream)
{
	var streamReader = new StreamReader(stream);
	var html = streamReader.ReadToEnd();
	var styleSelRegex = new System.Text.RegularExpressions.Regex(@"(?si)< *style *>.*</ *style *>");
	var unStyledHtml = styleSelRegex.Replace(html, " ");
	var tagSelRegex = new System.Text.RegularExpressions.Regex(@"<[^>]*>");
	var detaggedMsg = tagSelRegex.Replace(unStyledHtml, " ");
	var removeStrings = new string[] {"Get Online Help", "Reporting Services Error", "SQL Server Reporting Services"};
	removeStrings.ToList().ForEach(rmv => {detaggedMsg = detaggedMsg.Replace(rmv, " ");});
	var wsSelRegex = new System.Text.RegularExpressions.Regex(@"\s{2,}");
	var wsCleanedMsg = wsSelRegex.Replace(detaggedMsg, " ");
	var message = System.Web.HttpUtility.HtmlDecode(wsCleanedMsg);
	return message;
}



const string SampleHtml = @"<html>
        <head>
                <title>
                        SQL Server Reporting Services
                </title><meta name=""Generator"" content=""Microsoft SQL Server Reporting Services 16.0.1113.11"" />
<meta name=""HTTP Status"" content=""500"" />
<meta name=""ProductLocaleID"" content=""127"" />
<meta name=""CountryLocaleID"" content=""1033"" />
<meta name=""StackTrace"" content=""   at Microsoft.ReportingServices.OnDemandProcessing.AbortHelper.ThrowIfAborted(CancelationTrigger cancelationTrigger, String uniqueName)
   at Microsoft.ReportingServices.OnDemandProcessing.OnDemandProcessingContext.CheckAndThrowIfAborted()
   at Microsoft.ReportingServices.OnDemandProcessing.RetrievalManager.FetchData()
   at Microsoft.ReportingServices.OnDemandProcessing.RetrievalManager.PrefetchData(ReportInstance reportInstance, ParameterInfoCollection parameters, Boolean mergeTran)
   at Microsoft.ReportingServices.OnDemandProcessing.Merge.FetchData(ReportInstance reportInstance, Boolean mergeTransaction)
   at Microsoft.ReportingServices.ReportProcessing.Execution.ProcessReportOdpInitial.PreProcessSnapshot(OnDemandProcessingContext odpContext, Merge odpMerge, ReportInstance reportInstance, ReportSnapshot reportSnapshot)
   at Microsoft.ReportingServices.ReportProcessing.Execution.ProcessReportOdp.Execute(OnDemandProcessingContext&amp; odpContext)
   at Microsoft.ReportingServices.ReportProcessing.Execution.RenderReportOdpInitial.ProcessReport(ProcessingErrorContext errorContext, ExecutionLogContext executionLogContext, UserProfileState&amp; userProfileState)
   at Microsoft.ReportingServices.ReportProcessing.Execution.RenderReport.Execute(IRenderingExtension newRenderer)
   at Microsoft.ReportingServices.ReportProcessing.ReportProcessing.RenderReport(IRenderingExtension newRenderer, DateTime executionTimeStamp, ProcessingContext pc, RenderingContext rc, IChunkFactory yukonCompiledDefinition)
   at Microsoft.ReportingServices.ReportProcessing.ReportProcessing.RenderReport(DateTime executionTimeStamp, ProcessingContext pc, RenderingContext rc, IChunkFactory yukonCompiledDefinition)
   at Microsoft.ReportingServices.Library.RenderLive.CallProcessingAndRendering(ProcessingContext pc, RenderingContext rc, OnDemandProcessingResult&amp; result)
   at Microsoft.ReportingServices.Library.RenderStrategyBase.ExecuteStrategy(OnDemandProcessingResult&amp; processingResult)
   at Microsoft.ReportingServices.Library.ReportExecutionBase.InternalExecuteReport()
   at Microsoft.ReportingServices.Library.ReportExecutionBase.Execute()
   at Microsoft.ReportingServices.Diagnostics.CancelablePhaseBase.ExecuteWrapper()"" />
< style >
						BODY { FONT - FAMILY:'Segoe UI',Tahoma,Verdana,sans - serif; FONT - WEIGHT:normal; FONT - SIZE: 10pt; COLOR: black}
H1 { FONT - FAMILY:'Segoe UI',Tahoma,Verdana,sans - serif; FONT - WEIGHT:700; FONT - SIZE:15pt}
LI { FONT - FAMILY:'Segoe UI',Tahoma,Verdana,sans - serif; FONT - WEIGHT:normal; FONT - SIZE:10pt; DISPLAY: inline}
                        .ProductInfo { FONT - FAMILY:'Segoe UI',Tahoma,Verdana,sans - serif; FONT - WEIGHT:bold; FONT - SIZE: 10pt; COLOR: gray}
A: link {
	FONT - SIZE: 10pt; FONT - FAMILY:'Segoe UI',Tahoma,Verdana,sans - serif; COLOR:#3366CC; TEXT-DECORATION:none}
                        A: hover {
		FONT - SIZE: 10pt; FONT - FAMILY:'Segoe UI',Tahoma,Verdana,sans - serif; COLOR:#FF3300; TEXT-DECORATION:underline}
                        A: visited {
			FONT - SIZE: 10pt; FONT - FAMILY:'Segoe UI',Tahoma,Verdana,sans - serif; COLOR:#3366CC; TEXT-DECORATION:none}
                        A: visited: hover {
				FONT - SIZE: 10pt; FONT - FAMILY:'Segoe UI',Tahoma,Verdana,sans - serif; color:#FF3300; TEXT-DECORATION:underline}


				</ style >

		</ head >< body bgcolor = ""white"" >
   
				   < h1 >
						   Reporting Services Error<hr width= ""100%"" size= ""1"" color= ""silver"" />
   
				   </ h1 >< ul >
   
						   < li > An error has occurred during report processing. (rsProcessingAborted) < a href = ""https://go.microsoft.com/fwlink/?LinkId=20476&amp;EvtSrc=Microsoft.ReportingServices.Exceptions.ErrorStrings&amp;EvtID=rsProcessingAborted&amp;ProdName=Microsoft%20SQL%20Server%20Reporting%20Services&amp;ProdVer=16.0.1113.11"" target = ""_blank"" rel = ""noopener noreferrer"" > Get Online Help</ a ></ li >< ul >
			   
											   < li > Query execution failed for dataset &#39;Widgets&#39;. (rsErrorExecutingCommand) <a href=""https://go.microsoft.com/fwlink/?LinkId=20476&amp;EvtSrc=Microsoft.ReportingServices.Exceptions.ErrorStrings&amp;EvtID=rsErrorExecutingCommand&amp;ProdName=Microsoft%20SQL%20Server%20Reporting%20Services&amp;ProdVer=16.0.1113.11"" target=""_blank"" rel=""noopener noreferrer"">Get Online Help</a></li><ul>
                                        < li > Could not find stored procedure &#39;dbo.TestSp&#39;.</li>
                                </ ul >
				  
										  </ ul >
				  
								  </ ul >< hr width = ""100%"" size = ""1"" color = ""silver"" />< span class= ""ProductInfo"" > SQL Server Reporting Services </ span >
							   
									   </ body >
							   </ html >";
							   