<Query Kind="Program" />

void Main()
{
	var maxLength = 3;
	var firstPrintableAsciiCharCode = 32;
	var lastPrintableAsciiCharCode = 126;
	var charCode = new int[maxLength+1];
	
	do
	{
		for(var positionIndex = 0; positionIndex <= maxLength; positionIndex++)
		{
			if(charCode[positionIndex] == default(int)) 
				charCode[positionIndex] = firstPrintableAsciiCharCode;
			if(charCode[positionIndex] == lastPrintableAsciiCharCode)
			{
				charCode[positionIndex] = firstPrintableAsciiCharCode;
			}
			else
			{
				charCode[positionIndex]++;
				break;
			}
		}
		PrintWord(charCode);
	}
	while(charCode[maxLength] == default(int));
}

void PrintWord(int[] charCode)
{
	var sb = new StringBuilder();
	for(var i=charCode.Length-1; i >= 0; i--)
	{
		if(charCode[i] > default(int)) sb.Append((char)charCode[i]);
	}
	sb.ToString().Dump();
}
// Define other methods and classes here
