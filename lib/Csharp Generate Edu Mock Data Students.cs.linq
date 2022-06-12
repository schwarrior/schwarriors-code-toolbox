<Query Kind="Program">
  <Connection>
    <ID>9b622886-55a5-4aa9-a4f8-d00b1a0801a5</ID>
    <Server>.</Server>
    <Database>ISEE</Database>
    <DisplayName>ISEE</DisplayName>
    <Persist>true</Persist>
    <LinkedDb>SampleData</LinkedDb>
  </Connection>
</Query>

//--------------------------------------------------------------------------------------
// Generate Edu Mock Data
// Student Exam Registrations 
//--------------------------------------------------------------------------------------

// linqpad connection must be configured for ISEE db with "Include Additional Databases" checked. Add SampleData

void Main()
{
	var g = new EduMockDataGenerator(this);
	g.Generate();
}

class EduMockDataGenerator
{
	UserQuery _db;

	private Random __rnd = null;

	private Random _rnd
	{
		get
		{
			if (__rnd == null)
			{ __rnd = new Random(); }
			return __rnd;
		}
	}

	public EduMockDataGenerator(UserQuery db)
	{
		this._db = db;
	}
	
	public void Generate()
	{
		Console.WriteLine("Generate Edu Mock Data - Student Exam Registrations");

		// CheckClassConnections();

		var testSites = this.GetTestSites();
		testSites.Dump();

		Console.WriteLine("Done");
	}


	void CheckClassConnections()
	{
		_db.LabelStocks.Dump();
		_db.SampleData.Schools.Skip(200).Take(5).Dump();
	}


	int GetRandomInt(int minValue = 100000, int maxValue = 999999)
	{
		return _rnd.Next(minValue, maxValue + 1);
	}

	ICollection<LabelDoc> GetTestSites()
	{
		var LabelStockId = "A20L";
		var Level = "PRIMARY";
		var testDay = DateTime.Now.AddDays(7);
		while (testDay.DayOfWeek != DayOfWeek.Monday) { testDay = testDay.AddDays(1); }
		var TestDate = new DateTime(testDay.Year, testDay.Month, testDay.Day, 9, 0, 0);
		var shortYear = testDay.Year.ToString().Substring(2);
		var dateFolderName = $"{shortYear}{testDay.Month.ToString().PadLeft(2,'0')}{testDay.Day.ToString().PadLeft(2,'0')}";
		
		var lDocs = new List<LabelDoc>();
		var schools = _db.SampleData.Schools.Skip(300).Take(15).ToList();
		
		foreach (var s in schools)
		{
			var SiteCode = GetRandomInt();
			var SiteName = s.SchoolName;
			var SaveFilePath = $"{dateFolderName}\\{SiteCode}.pdf";
			
			var lDoc = new LabelDoc();
			lDoc.LabelStockId = LabelStockId;
			lDoc.TestDate = TestDate;
			lDoc.SiteCode = SiteCode;
			lDoc.SaveFilePath = SaveFilePath;
			lDocs.Add(lDoc);
		} 
		return lDocs;
	} 

}