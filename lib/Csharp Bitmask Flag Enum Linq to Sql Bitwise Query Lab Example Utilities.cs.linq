<Query Kind="Program">
  <Connection>
    <ID>d68f514f-c74d-4822-88dc-2bc697533a4d</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>SampleData</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

void Main()
{
	//EvaluateStatusFlags();
	//LinqToObjectTest();	
	//InitializeLinqToSqlTest();
	//SimpleLinqToSqlTest();
	AdvancedLinqToSqlTest();
}

[Flags]
public enum SeverityLvl
{
	  Off = 0,
      Error = 1,
      Warning = 2,
	  Info = 4,
	  Verbose = 8,
	  Custom = 16
}

List<SeverityLvl> _severityLvls = new List<SeverityLvl> 
{
	{ SeverityLvl.Error}, //1
	{ SeverityLvl.Error | SeverityLvl.Warning}, //3
	{ SeverityLvl.Error | SeverityLvl.Verbose | SeverityLvl.Info }, //13
	{ SeverityLvl.Error | SeverityLvl.Custom | SeverityLvl.Info }, //21
	{ SeverityLvl.Error | SeverityLvl.Warning | SeverityLvl.Info }, //7
	{ SeverityLvl.Info | SeverityLvl.Error | SeverityLvl.Warning } //7 //same as above, just different order. Doesn't matter. Always displayed processed in order
};

void AdvancedLinqToSqlTest()
{
	"Possible Int Values Containing bitmask values:".Dump();
	var findFlags = SeverityLvl.Error | SeverityLvl.Warning;
	findFlags.Dump();
	
	"Values in DB found containing same bitmask values:".Dump();
	var possibleVals = Utilities.GetAllPosibleEnumFlagIntegers<SeverityLvl>(findFlags).ToList();
	possibleVals.Dump();
	
	var essayImageCreatedStatuses = from sl in SeverityLevels 
									where possibleVals.Contains(sl.LevelValue)
									select sl;
	essayImageCreatedStatuses.Dump();
	
}

void SimpleLinqToSqlTest()
{
	//fails: (will not compile)
	//var warningStatuses = from rs in SeverityLevel where ((rs.SeverityLevel & SeverityLvl.Warning) == SeverityLvl.Warning) select rs;
	//warningStatuses.Dump();
	"Conclusion: cannot do bitwise operations inside linq to sql query".Dump();
}

void InitializeLinqToSqlTest()
{
	//identify a sql db to use for testing
	//run following sql to create table
		//Create Table SeverityLevel(id int not null primary key, LevelValue int not null, LevelName varchar(25) not null)
	//connect database to this linqPad program
	var severityLevels = _severityLvls.Select(lvl => new SeverityLevel() {LevelValue = (int)lvl, LevelName = lvl.ToString()});
	SeverityLevels.InsertAllOnSubmit(severityLevels);
	//severityLevels.Dump();
	SubmitChanges();
}

void LinqToObjectTest()
{
	var warningStatuses = from rs in _severityLvls where ((rs & SeverityLvl.Warning) == SeverityLvl.Warning) select rs;
	warningStatuses.Dump();
}

void EvaluateStatusFlags()
{
	foreach(var severityLvl in _severityLvls)
	{
		severityLvl.Dump();
		string.Empty.Dump();

		SeverityLvl.Error.Dump();
		((severityLvl & SeverityLvl.Error) == SeverityLvl.Error).Dump();
		
		SeverityLvl.Info.Dump();
		((severityLvl & SeverityLvl.Info) == SeverityLvl.Info).Dump();
	
		SeverityLvl.Verbose.Dump();
		((severityLvl & SeverityLvl.Verbose) == SeverityLvl.Verbose).Dump();
		
		(SeverityLvl.Error | SeverityLvl.Info).Dump();
		((severityLvl & (SeverityLvl.Error | SeverityLvl.Info)) == (SeverityLvl.Error | SeverityLvl.Info)).Dump();
		
		(SeverityLvl.Error | SeverityLvl.Info | SeverityLvl.Warning).Dump();
		((severityLvl & (SeverityLvl.Error | SeverityLvl.Info | SeverityLvl.Warning)) == (SeverityLvl.Error | SeverityLvl.Info | SeverityLvl.Warning)).Dump();
		
		(SeverityLvl.Error | SeverityLvl.Verbose).Dump();
		((severityLvl & (SeverityLvl.Error | SeverityLvl.Verbose)) == (SeverityLvl.Error | SeverityLvl.Verbose)).Dump();	
		
		string.Empty.Dump();
	}
}

public class Utilities
{
	public static IEnumerable<int> GetAllPosibleEnumFlagIntegers<T>(T enumFlags) where T : struct, IConvertible
	{
		var possibleInts = new List<int>();
		var enumFlagType = typeof(T);
		var enumFlagsInt = (int)Convert.ChangeType(enumFlags, typeof(int));
		int sumOfAllFlagInts;
		GetEnumFlagValueLimit<T>(out sumOfAllFlagInts);
		for(var val = 1; val <= sumOfAllFlagInts; val++)
		{
			var hasFlags = ((val & enumFlagsInt) == enumFlagsInt);
			if (hasFlags) possibleInts.Add(val);
		}
		return possibleInts;
	}

	public static int GetEnumFlagValueLimit<T>(out int sumOfAllValues) where T : struct, IConvertible
	{
		var maxValueInt = 0;
		sumOfAllValues = 0;
		var enumFlagType = typeof(T);
		var enumObjectArray = Enum.GetValues(enumFlagType);
		foreach(var enumObject in enumObjectArray)
		{
			var enumValue = (int)enumObject;
			sumOfAllValues += enumValue;
			if(enumValue>maxValueInt) maxValueInt = enumValue;
		}
		var maxValueIntChecksum = Math.Pow(2, (enumObjectArray.Length-2));
		Debug.Assert(maxValueInt == maxValueIntChecksum, "Flag enum may not be configured correctly");
		Debug.Assert(sumOfAllValues == (maxValueInt*2)-1, "Flag enum may not be configured correctly");
		return maxValueInt;
	}

}