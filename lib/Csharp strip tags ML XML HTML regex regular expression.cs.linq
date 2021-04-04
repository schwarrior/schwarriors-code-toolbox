<Query Kind="Program" />

void Main()
{
	Console.WriteLine("Csharp strip tags ML XML HTML regex regular expression");
	Console.WriteLine();
	
	var inString = "<?xml><a>hello<b atrib=\"0\">world</b></a>";
	Console.WriteLine(inString);
	Console.WriteLine(RemoveTags(inString));
}

string RemoveTags(string xmlInString)
{
	
	//in java per https://stackoverflow.com/questions/15769028/java-regex-to-strip-out-xml-tags-but-not-tag-contents
	//"How now <fizz>brown</fizz> cow.".replaceAll("<[^>]+>", "")
	
	string pattern = "<[^>]+>";
    var result = Regex.Replace(xmlInString, pattern, " ");
    return result.Trim();
}
