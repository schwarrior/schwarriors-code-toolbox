<Query Kind="Program" />

void Main()
{
	//Func<> returns the type specified as the final generic type parameter, such that Func<int> returns an int and Func<int, string> accepts an integer and returns a string. Examples:
	Func<int> getOne = () => 1;
	Func<int, string> convertIntToString = i => i.ToString();
	Action<string> printToScreen = s => Console.WriteLine(s);
	// use them
	printToScreen(convertIntToString(getOne()));
}
// Define other methods and classes here
