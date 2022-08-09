<Query Kind="Program" />

void Main()
{
	GenerateSqlForPresidentLabels();
}

void GenerateSqlForPresidentLabels()
{
	var pageOrd = 1;
	var colOrd = 1;
	var rowOrd = 1;
	for (var prezOrd = 1; prezOrd <= Presidents.Length; prezOrd++)
	{
		var sb = new StringBuilder();
		var prezName = Presidents[prezOrd - 1];
		if (prezOrd > 1) { sb.Append("Union "); }
		sb.Append($"Select {pageOrd} as [Page], {colOrd} as [Col], {rowOrd} as [Row], {prezOrd} as PrezNum, '{prezName}' as PrezName");
		if (prezOrd == Presidents.Length) { sb.Append("\r\nOrder By PrezNum;\r\n"); }
		Console.WriteLine(sb.ToString());
		//colOrd = prezOrd % colLength + 1;
		//rowOrd = prezOrd % rowLength + 1;
		//pageOrd = prezOrd / (colLength * rowLength) + 1;
		colOrd++;
		colOrd = (colOrd - 1) % colLength + 1;
		if (colOrd == 1) { rowOrd++; }
		rowOrd = (rowOrd - 1) % rowLength + 1;
		if (colOrd == 1 && rowOrd == 1) { pageOrd++; }
	}
}

const int colLength = 2;
const int rowLength = 7;

string[] Presidents = {
		"George Washington", "John Adams", "Thomas Jefferson", "James Madison", "James Monroe", "John Quincy Adams", "Andrew Jackson", "Martin Van Buren", "William Henry Harrison", "John Tyler", "James K. Polk", "Zachary Taylor", "Millard Fillmore", "Franklin Pierce", "James Buchanan", "Abraham Lincoln", "Andrew Johnson", "Ulysses S. Grant", "Rutherford B. Hayes", "James A. Garfield", "Chester A. Arthur", "Grover Cleveland", "Benjamin Harrison", "Grover Cleveland", "William McKinley", "Theodore Roosevelt", "William Howard Taft", "Woodrow Wilson", "Warren G. Harding", "Calvin Coolidge", "Herbert Hoover", "Franklin D. Roosevelt", "Harry S. Truman", "Dwight D. Eisenhower", "John F. Kennedy", "Lyndon B. Johnson", "Richard Nixon", "Gerald Ford", "Jimmy Carter", "Ronald Reagan", "George H. W. Bush", "Bill Clinton", "George W. Bush", "Barack Obama", "Donald J. Trump", "Joe Biden"
	};