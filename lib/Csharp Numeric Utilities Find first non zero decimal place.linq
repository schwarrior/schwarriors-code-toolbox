<Query Kind="Program" />

void Main()
{
	testThis(0F);
	testThis(0.00000024937F);
	testThis(0.00000092341F);
	testThis(0.456F);
	testThis(0.8782376499201F);
	testThis((float)Math.PI);
}

void testThis(float value)
{
	value.Dump();
	var digits = findFirstNonZeroDecimalPlace(value,3,12);
	digits.Dump();
	var roundValue = Math.Round(value,digits);
	roundValue.Dump();
	var formatString = generateFomatStringForDigits(digits);
	formatString.Dump();
	value.ToString(formatString).Dump();
	
	string.Empty.Dump();
}

// Define other methods and classes here
int findFirstNonZeroDecimalPlace(float value, int minDigits, int maxDigits){
	if (value==0F) return minDigits;
	int digits;
	for(digits = minDigits; digits <= maxDigits; digits++)
	{
		var roundValue = Math.Round(value,digits);
		if(roundValue > 0F) break;
	}
	return digits;
}

string generateFomatStringForDigits(int digits)
{
	var fs = new StringBuilder();
	fs.Append("0");
	if(digits>0) fs.Append(".");
	fs.Append(String.Empty.PadLeft(digits, '0'));
	return fs.ToString();
}
