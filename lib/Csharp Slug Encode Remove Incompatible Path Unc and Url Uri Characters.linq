<Query Kind="Program" />

void Main()
{
	var inStrs = new List<string> 
	{
		"http://blogs.msdn.microsoft.com/patrickdanino/2008/07/23/user-settings-in-wpf",
		@"C:\Users\Public\Document\FileA.txt"
	};
	inStrs.ForEach(inStr => 
	{
		SlugEncode(inStr).Dump();
	}
	);
}

// Define other methods and classes here
public static string SlugEncode(string inString)
{
    var sb = new StringBuilder();
    var invalidChars = Path.GetInvalidFileNameChars();
    foreach (var inStringChar in inString)
    {
        if (invalidChars.Contains(inStringChar))
        {
			if(sb.ToString().ToCharArray().Last() == '-') continue;
            sb.Append("-");
        }
        else
        {
            sb.Append(inStringChar);
        }
    }
    return sb.ToString();
}