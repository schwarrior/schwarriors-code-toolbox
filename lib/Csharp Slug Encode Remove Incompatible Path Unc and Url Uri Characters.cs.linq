<Query Kind="Program" />

void Main()
{
	var samples = new List<string> 
	{
		"https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/indexed_collections#array_methods",
		"---hay stack---",
		@"C:\Users\Public\Document\FileA.txt",
		"¿It’s a T–Bone?"
	};
	samples.ForEach(sample => 
	{
		Console.WriteLine(sample);
		Console.WriteLine(SlugEncode(sample, lowerCase: true));
		Console.WriteLine();
	});
}

/// <summary>
/// Replaces every character that is not strictly alphanumeric with a dash
/// </summary>
public static string SlugEncode(
	string inString,
	bool lowerCase = false,
	bool dedupeSlugs = true,
	bool trimSlugs = true,
	char slugChar = '-',
	string allowedChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
)
{
	var sbOut = new StringBuilder();
	var inStrFormat = lowerCase ? inString.ToLower() : inString;
	var inStrLen = inStrFormat.Length;
	for(int inIdx = 0; inIdx < inStrLen; inIdx ++)
    {
		var inChar = inStrFormat[inIdx];
		if (allowedChars.Contains(inChar))
		{
			sbOut.Append(inChar);
			continue;
		}
		if (trimSlugs && (sbOut.Length == 0 || inIdx >= inStrLen-1)) continue;
		if (dedupeSlugs) 
		{
			var lastOutChar = sbOut.ToString().LastOrDefault();
			if (lastOutChar == slugChar) continue;
		}
		sbOut.Append(slugChar);
    }
	if (sbOut.ToString().LastOrDefault() == slugChar) sbOut.Remove(sbOut.Length-1, 1);
    return sbOut.ToString();
}