<Query Kind="Program" />

void Main()
{
	var wilcoPairs = new List<WilcoxonTestValuePair>{
		new WilcoxonTestValuePair{Rank = 1},
		new WilcoxonTestValuePair{Rank = 2},
		new WilcoxonTestValuePair{Rank = 2},
		new WilcoxonTestValuePair{Rank = 3},
		new WilcoxonTestValuePair{Rank = 3},
		new WilcoxonTestValuePair{Rank = 4},
	};
	
//	var groupByResults = from p in wilcoPairs
//	                group p by p.Rank
//	                into g
//	                select new {Rank = g.Key, Pairs = g};
//	groupByResults.Dump();
	
	var groupByHavingResults = from p in wilcoPairs
	                group p by p.Rank
	                into g
					where g.Count() > 1
	                select new {Rank = g.Key, Pairs = g};
	
	//access the grouped items
	foreach(var grp in groupByHavingResults)
	{
		//grp.Dump();
		foreach(var item in grp.Pairs)
		{
			item.Note = "dupe";
			//item.Dump();
		}
	}
	
	wilcoPairs.Dump();
}

public class WilcoxonTestValuePair
{
	public string Note {get; set; }
	public double Rank { get; set; }
}