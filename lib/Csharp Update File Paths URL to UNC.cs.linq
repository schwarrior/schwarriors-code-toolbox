<Query Kind="Program" />

void Main()
{
	updateFilePathsUrlToUnc(
		inFileName: @"\\HomeMac\Music\iTunes\iTunesMusicLibrary.xml", 
		outFileName: @"\\HomeMac\Music\iTunes\iTunesMusicLibrary.net.xml");
}

void updateFilePathsUrlToUnc(string inFileName, string outFileName)
{
	using (var outFile = File.CreateText(outFileName))
	{
		// Read the file and display it line by line.  
		string line;
		using (var file = new System.IO.StreamReader(inFileName))
		{
			while ((line = file.ReadLine()) != null)
			{
				var bodyStart = line.IndexOf("file:///");
				if (bodyStart < 0)
				{
					outFile.WriteLine(line.TrimEnd());
					continue;
				}
				var suffixStart = line.IndexOf("</string>");
				var prefix = line.Substring(0, bodyStart);
				var body = line.Substring(bodyStart, suffixStart - bodyStart);
				var suffix = line.Substring(suffixStart).TrimEnd();

				body = Uri.UnescapeDataString(body);
				body = body.Replace("file:///Users/schwarrior/Music/", "\\\\HomeMac\\Music\\");
				body = body.Replace("/", "\\");

				var newline = prefix + body + suffix;

				outFile.WriteLine(newline);
				Console.WriteLine(body);
			}
			file.Close();
		}
		outFile.Close();
	}
}