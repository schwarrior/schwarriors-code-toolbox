<Query Kind="Program" />

void Main()
{
	getPageAndPageItemOrdinals(1).Dump();
	getPageAndPageItemOrdinals(11).Dump();
	getPageAndPageItemOrdinals(19).Dump();
	getPageAndPageItemOrdinals(20).Dump();
	getPageAndPageItemOrdinals(21).Dump();
}

const int labelsPerPage = 20;

Tuple<int, int> getPageAndPageItemOrdinals(int absoluteOrdinal)
{
	if (absoluteOrdinal < 1)
	{
		throw new ArgumentOutOfRangeException("Absolute ordinal cannot be less than 1");
	}
	var page = (int)Math.Ceiling((double)absoluteOrdinal / labelsPerPage);
	var pageItem = absoluteOrdinal % labelsPerPage;
	if (pageItem == 0) { pageItem = labelsPerPage; }
	var ords = new Tuple<int, int>(page, pageItem);
	return ords;
}