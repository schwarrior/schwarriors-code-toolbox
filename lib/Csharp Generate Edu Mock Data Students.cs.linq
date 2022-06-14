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

	private IList<string> __firstNames = null;

	private IList<string> _firstNames
	{
		get
		{
			if (__firstNames == null)
			{ 
				__firstNames = _db.SampleData.FirstNames.Select(fn => fn.Content).ToList();
			}
			return __firstNames;
		}
	}

	private IList<string> __lastNames = null;

	private IList<string> _lastNames
	{
		get
		{
			if (__lastNames == null)
			{
				__lastNames = _db.SampleData.lastNames.Select(fn => fn.Content).ToList();
			}
			return __lastNames;
		}
	}

	public EduMockDataGenerator(UserQuery db)
	{
		this._db = db;
	}
	
	public void Generate()
	{
		Console.WriteLine("Generate Edu Mock Data - Student Exam Registrations");

		CheckClassConnections();

		Console.WriteLine("Selecting test sites");
		var testSites = this.GetTestSites();		
		testSites.Dump();

		foreach(var site in testSites)
		{
			Console.WriteLine($"Saving site {site.SiteName} to DB as LabelDoc record");
			var savedSiteCount = this.SaveTestSite(site);
			Console.WriteLine($"Saved {savedSiteCount} site{ ( savedSiteCount == 0 || savedSiteCount > 1 ? "s" : string.Empty )} to DB");

			Console.WriteLine($"Selecting students for test site {site.SiteName}");
			var students = this.GetStudentsForSite(site);
			Console.WriteLine("Selected students:");
			students.Dump();
			Console.WriteLine("Saving students to DB as Label records");
			var savedStuCount = this.saveTestSiteStudents(site, students);
			Console.WriteLine($"Saved {savedStuCount} student{ ( savedStuCount == 0 || savedStuCount > 1 ? "s" : string.Empty )} to DB");
		}

		Console.WriteLine("Done");
	}


	void CheckClassConnections()
	{
		Console.WriteLine("Checking DB Connectivity from class");
		_db.LabelStocks.Dump();
		// _db.SampleData.Schools.Skip(200).Take(5).Dump();
		this._firstNames.Take(5).Dump();
		Console.WriteLine("Connectivity verified");
	}


	int GetRandomInt(int minValue = 100000, int maxValue = 999999)
	{
		return _rnd.Next(minValue, maxValue + 1);
	}

	ICollection<LabelDoc> GetTestSites(int siteCount = 5)
	{
		var LabelStockId = "A20L";
		var Level = "PRIMARY";
		var testDay = DateTime.Now.AddDays(7);
		while (testDay.DayOfWeek != DayOfWeek.Monday) { testDay = testDay.AddDays(1); }
		var TestDate = new DateTime(testDay.Year, testDay.Month, testDay.Day, 9, 0, 0);
		var shortYear = testDay.Year.ToString().Substring(2);
		var dateFolderName = $"{shortYear}{testDay.Month.ToString().PadLeft(2,'0')}{testDay.Day.ToString().PadLeft(2,'0')}";
		
		var lDocs = new List<LabelDoc>();
		var skip = GetRandomInt(0, _db.SampleData.Schools.Count() - 1 - siteCount);
		var schools = _db.SampleData.Schools.Skip(skip).Take(siteCount).ToList();
		
		foreach (var s in schools)
		{
			var SiteCode = GetRandomInt();
			var SiteName = s.SchoolName;
			var SaveFilePath = $"{dateFolderName}\\{SiteCode}.pdf";
			
			var lDoc = new LabelDoc();
			
			lDoc.LabelStockId = LabelStockId;
			lDoc.TestDate = TestDate;
			lDoc.SiteCode = SiteCode;
			lDoc.SiteName = SiteName;
			lDoc.SaveFilePath = SaveFilePath;
			lDocs.Add(lDoc);
		} 
		return lDocs;
	}

	int SaveTestSite(LabelDoc testSites)
	{
		return 0;
	}

	ICollection<Label> GetStudentsForSite(LabelDoc site)
	{
		var lbls = new List<Label>();
		return lbls;
	}
	
	int saveTestSiteStudents(LabelDoc site, ICollection<Label> students) {
		return 0;
	}

}