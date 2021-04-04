<Query Kind="Program" />

void Main()
{
	const string path = @"C:\Users\Public\Documents";
	var files = Directory.GetFiles(path,"*.*",SearchOption.AllDirectories);
	var rnd = new Random();
	var index = rnd.Next(0, files.Length);
	files[index].Dump();
}
