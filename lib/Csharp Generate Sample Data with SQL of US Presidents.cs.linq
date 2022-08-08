<Query Kind="Program" />


const int colLength = 2;
const int rowLength = 10;

void Main()
{
	var pageIdx = 0;
	var colIdx = 0;
	var rowIdx = 0;
	for (var prezNum = 1; prezNum <= Presidents.Length; prezNum ++)
	{
		var sb = new StringBuilder ();
		var prezName = Presidents [prezNum - 1];
		if (prezNum > 1) { sb.Append("Union "); }
		sb.Append($"Select {pageIdx+1} as [Page], {colIdx+1} as [Col], {rowIdx+1} as [Row], {prezNum} as PrezNum, '{prezName}' as PrezName");
		if (prezNum == Presidents.Length) { sb.Append ("\r\nOrder By PrezNum;"); }
		Console.WriteLine (sb.ToString());
		colIdx++;
		colIdx = colIdx % colLength;
		if (colIdx == 0) { rowIdx++; }
		rowIdx = rowIdx % rowLength;
		if (colIdx == 0 && rowIdx == 0) { pageIdx++; }
	}
}

// Define other methods and classes here
string[] Presidents = {
		"George Washington", "John Adams", "Thomas Jefferson", "James Madison", "James Monroe", "John Quincy Adams", "Andrew Jackson", "Martin Van Buren", "William Henry Harrison", "John Tyler", "James K. Polk", "Zachary Taylor", "Millard Fillmore", "Franklin Pierce", "James Buchanan", "Abraham Lincoln", "Andrew Johnson", "Ulysses S. Grant", "Rutherford B. Hayes", "James A. Garfield", "Chester A. Arthur", "Grover Cleveland", "Benjamin Harrison", "Grover Cleveland", "William McKinley", "Theodore Roosevelt", "William Howard Taft", "Woodrow Wilson", "Warren G. Harding", "Calvin Coolidge", "Herbert Hoover", "Franklin D. Roosevelt", "Harry S. Truman", "Dwight D. Eisenhower", "John F. Kennedy", "Lyndon B. Johnson", "Richard Nixon", "Gerald Ford", "Jimmy Carter", "Ronald Reagan", "George H. W. Bush", "Bill Clinton", "George W. Bush", "Barack Obama", "Donald J. Trump", "Joe Biden"
	};