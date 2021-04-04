<Query Kind="Program" />

void Main()
{
	const string path = @"C:\Temp\";
	var files = Directory.GetFiles(path,"*.*",SearchOption.AllDirectories);
	foreach(var file in files)
	{
		var pathWithoutRoot = file.Replace(path,string.Empty);
		var tsv = pathWithoutRoot.Replace("\\","\t");
		tsv = tsv.Replace(".txt",string.Empty);
		tsv.Dump();
	}
}

// Define other methods and classes here
