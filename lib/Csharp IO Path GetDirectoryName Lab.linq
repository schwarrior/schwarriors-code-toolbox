<Query Kind="Program" />

void Main()
{
	var dir = @"C:\Dev\SC\MeasIncSvn\ISEE\Ichigo\Publish Ichigo Help.txt";
	dir.Dump();
	System.IO.Path.GetDirectoryName(dir).Dump();
}

// Define other methods and classes here
