<Query Kind="Program" />

void Main()
{
	var page = 1;
	for (var presNum = 1; presNum <= Presidents.Length; presNum ++)
	{
		var sb = new StringBuilder ();
		var presName = Presidents [presNum - 1];
		if (presNum > 1) { sb.Append ("Union "); }
		sb.Append ($"Select {page} as PageNum, {presNum} as PrezNum, '{presName}' as PrezName");
		if (presNum == Presidents.Length) { sb.Append (";"); }
		Console.WriteLine (sb.ToString());
		if (presNum % 10 == 0) { page++; }
	}
}

// Define other methods and classes here
string[] Presidents = {
		"George Washington", "John Adams", "Thomas Jefferson", "James Madison", "James Monroe", "John Quincy Adams", "Andrew Jackson", "Martin Van Buren", "William Henry Harrison", "John Tyler", "James K. Polk", "Zachary Taylor", "Millard Fillmore", "Franklin Pierce", "James Buchanan", "Abraham Lincoln", "Andrew Johnson", "Ulysses S. Grant", "Rutherford B. Hayes", "James A. Garfield", "Chester A. Arthur", "Grover Cleveland", "Benjamin Harrison", "Grover Cleveland", "William McKinley", "Theodore Roosevelt", "William Howard Taft", "Woodrow Wilson", "Warren G. Harding", "Calvin Coolidge", "Herbert Hoover", "Franklin D. Roosevelt", "Harry S. Truman", "Dwight D. Eisenhower", "John F. Kennedy", "Lyndon B. Johnson", "Richard Nixon", "Gerald Ford", "Jimmy Carter", "Ronald Reagan", "George H. W. Bush", "Bill Clinton", "George W. Bush", "Barack Obama", "Donald J. Trump", "Joe Biden"
	};