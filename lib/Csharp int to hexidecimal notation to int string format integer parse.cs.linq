<Query Kind="Program" />

void Main()
{
	int intValue = 15;
	Console.WriteLine(intValue.ToString());
	string hexValue = intValue.ToString("X");
	Console.WriteLine(hexValue);
	int intAgain = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
	Console.WriteLine(intAgain.ToString());
}

// Define other methods and classes here
