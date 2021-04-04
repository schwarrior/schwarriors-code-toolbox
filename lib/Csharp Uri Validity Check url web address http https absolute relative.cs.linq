<Query Kind="Program" />

void Main()
{
	Test("hello");
	Test("hello world");
	Test("hello/world");
	Test("/hello");
	Test(@"hello\world");
	Test("http://helloworld.com");
	Test("https://helloworld.com");
	Test("//helloworld.com");
}

public void Test(string uri)
{
	uri.Dump();
	"IsValidAbsoluteUri".Dump();
	IsValidAbsoluteUri(uri).Dump();
	"IsValidRelativeUri".Dump();
	IsValidRelativeUri(uri).Dump();
	string.Empty.Dump();
}

public bool IsValidAbsoluteUri(string uri)
{
    Uri validatedUri;
    return Uri.TryCreate(uri, UriKind.Absolute, out validatedUri);
}

public bool IsValidRelativeUri(string uri)
{
	if(uri.Any(Char.IsWhiteSpace)) return false;
	Uri validatedUri;
    return Uri.TryCreate(uri, UriKind.Relative, out validatedUri);
}