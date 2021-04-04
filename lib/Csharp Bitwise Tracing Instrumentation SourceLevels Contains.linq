<Query Kind="Program" />

void Main()
{
	//get current tracelevel in app that uses .net tracing
	//TraceSource traceSource = new TraceSource("MyTraceSourceNameFromAppConig");
	//var currentLevel = traceSource.Switch.Level;
	
	SourceLevelContains(SourceLevels.All, SourceLevels.Verbose);
	SourceLevelContains(SourceLevels.Verbose, SourceLevels.Verbose);
	SourceLevelContains(SourceLevels.Information, SourceLevels.Verbose);
	SourceLevelContains(SourceLevels.Warning, SourceLevels.Verbose);
	
	SourceLevelContains(SourceLevels.Verbose, SourceLevels.All);
	SourceLevelContains(SourceLevels.Information, SourceLevels.Warning);
	
}

bool SourceLevelContains(SourceLevels sourceLevel, SourceLevels containsLevel)
{
	("Source Level: " + sourceLevel.ToString()).Dump();
	("Contains : " + containsLevel.ToString()).Dump();
	var contains = (sourceLevel & containsLevel) == containsLevel;
	contains.Dump();
	"".Dump();
	return contains;
}