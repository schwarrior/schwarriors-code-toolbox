/// <summary>
/// Enhances System.IO.Path.Combine by supporting URI paths (forward slashes)
/// Also, fixes bug where path2 is dropped when combining a path1 with a trailing slash and a path2 with a leading slash
/// </summary>
public static string PathCombineWithUriSupport(string path1, string path2)
{
    var isUri = path1.Contains('/') || path2.Contains('/');
    var path1Unc = isUri ? path1.Replace('/', '\\') : path1;
    var path2Unc = isUri ? path2.Replace('/', '\\') : path2;
    if (path1Unc.EndsWith("\\")) path1Unc = path1Unc.Substring(0, path1Unc.Length - 1);
    if (path2Unc.StartsWith("\\")) path2Unc = path2Unc.Substring(1);
    var combinedUncPath = Path.Combine(path1Unc, path2Unc);
    var combinedPath = isUri ? combinedUncPath.Replace('\\', '/'): combinedUncPath;
    return combinedPath;
}

void Main()
{
	//PathCombineWithUriSupport tests
	Console.WriteLine(System.IO.Path.Combine(@"//google.com","gmail")); //injects UNC backslash in URI 
	Console.WriteLine(PathCombineWithUriSupport(@"//google.com","gmail"));
	Console.WriteLine(System.IO.Path.Combine(@"\\myDir\SubDir\",@"\New Stuff\Today\")); //drops second path
	Console.WriteLine(PathCombineWithUriSupport(@"\\myDir\SubDir\",@"\New Stuff\Today\")); 	
}

