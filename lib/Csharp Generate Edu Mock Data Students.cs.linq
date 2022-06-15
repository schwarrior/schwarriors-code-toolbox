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
	g.generate();
}

class EduMockDataGenerator
{

	const int labelsPerPage = 20;

	const double genericsPerPreId = .25;

	const string labelStockId = "A20L";

	const int daysBeforelabelDocExpires = 200;
	
	const int numSampleTestSiteDocs = 3;

	/// 0 = YYMMDD test date folder, 1 = site name, 2 = site code
	const string saveFileFormatStr = "{0}\\Labels-Primary-{1}-{2}.pdf";

	/// 0 = grade number. Either "2", "3" or "4"
	const string gradeLevelFormatStr = "PRIMARY{0}";

	const string genericLabelStudentName = "Walk In";

	/// LinqPad is unable to let SQL Server alone to set default values on inserted entities
	const bool populateSqlColsWithDefaultValues = true;

	public EduMockDataGenerator(UserQuery db)
	{
		this._db = db;
	}

	public void generate()
	{

		var minSampleStudentsPerDoc = Convert.ToInt32 ( Math.Ceiling ( labelsPerPage / 4.0 ) );
		var maxSampleStudentsPerDoc = Convert.ToInt32 ( Math.Floor ( labelsPerPage * 2.5 ) );

		Console.WriteLine("Generate Edu Mock Data - Student Exam Registrations");

		checkClassConnections();

		Console.WriteLine("Selecting test sites");
		var testSites = this.getTestSites(numSampleTestSiteDocs);
		testSites.Dump();

		foreach (var site in testSites)
		{
			Console.WriteLine($"Saving site {site.SiteName} to DB as LabelDoc record");
			var savedSiteCount = this.saveTestSite(site);
			Console.WriteLine($"Saved {savedSiteCount} site{ (savedSiteCount == 0 || savedSiteCount > 1 ? "s" : string.Empty)} to DB");
			Console.WriteLine($"Selecting students for test site {site.SiteName}");
			var rndStudCount = getRandomInt(minSampleStudentsPerDoc, maxSampleStudentsPerDoc);
			var appendGenericCount = Convert.ToInt32(Math.Ceiling(rndStudCount*genericsPerPreId));
			var studLabels = getPreIdStudentsForSite(site, rndStudCount);
			appendGenericLabelsForSite(site, appendGenericCount, ref studLabels);
			Console.WriteLine("Selected students:");
			studLabels.Dump();
			Console.WriteLine("Saving students to DB as Label records");
			var savedStuCount = this.saveTestSiteStudents(studLabels);
			Console.WriteLine($"Saved {savedStuCount} student{ (savedStuCount == 0 || savedStuCount > 1 ? "s" : string.Empty)} to DB");
		}

		Console.WriteLine("Done");
	}

	UserQuery _db;

	Random __rnd = null;

	Random _rnd
	{
		get
		{
			if (__rnd == null)
			{ __rnd = new Random(); }
			return __rnd;
		}
	}

	IList<string> __firstNames = null;

	IList<string> _firstNames
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

	string getRandomFirstName() 
	{
		var moveIndex = getRandomInt(0, _firstNames.Count() - 1);
		return _firstNames[moveIndex];
	}

	IList<string> __lastNames = null;

	IList<string> _lastNames
	{
		get
		{
			if (__lastNames == null)
			{
				__lastNames = _db.SampleData.LastNames.Select(fn => fn.Content).ToList();
			}
			return __lastNames;
		}
	}

	string getRandomLastName()
	{
		var moveIndex = getRandomInt(0, _lastNames.Count() - 1);
		return _lastNames[moveIndex];
	}




	void checkClassConnections()
	{
		Console.WriteLine("Checking DB Connectivity from class");
		_db.LabelStocks.Take(numSampleTestSiteDocs).Dump();
		this._firstNames.Take(numSampleTestSiteDocs).Dump();
		Console.WriteLine("Connectivity verified");
	}


	int getRandomInt(int minValue = 100000, int maxValue = 999999)
	{
		return _rnd.Next(minValue, maxValue + 1);
	}

	ICollection<LabelDoc> getTestSites(int siteCount)
	{
		var testDay = DateTime.Now.AddDays(7);
		while (testDay.DayOfWeek != DayOfWeek.Monday) { testDay = testDay.AddDays(1); }
		var TestDate = new DateTime(testDay.Year, testDay.Month, testDay.Day, 9, 0, 0);
		var shortYear = testDay.Year.ToString().Substring(2);
		var dateFolderName = $"{shortYear}{testDay.Month.ToString().PadLeft(2,'0')}{testDay.Day.ToString().PadLeft(2,'0')}";
		
		var lDocs = new List<LabelDoc>();
		var skip = getRandomInt(0, _db.SampleData.Schools.Count() - 1 - siteCount);
		var schools = _db.SampleData.Schools.Skip(skip).Take(siteCount).ToList();
		
		foreach (var s in schools)
		{
			var SiteCode = getRandomInt();
			var SiteName = s.SchoolName;
			var SaveFilePath = string.Format(saveFileFormatStr, dateFolderName, slugEncode(SiteName), SiteCode);
			
			var lDoc = new LabelDoc();
			
			lDoc.LabelStockId = labelStockId;
			lDoc.TestDate = TestDate;
			lDoc.SiteCode = SiteCode;
			lDoc.SiteName = SiteName;
			lDoc.SaveFilePath = SaveFilePath;
			
			// ordinarily these should be left unset and default values would be handled by SQL 
			// these must be defined because of LinqPad limitation
			if (populateSqlColsWithDefaultValues)
			{
				lDoc.CreateDate = DateTime.Now;
				lDoc.ExpireDate = DateTime.Now.AddDays(daysBeforelabelDocExpires);
				lDoc.CreateUser = Environment.UserName;
				lDoc.CreateHost = Environment.MachineName;
			}

			lDocs.Add(lDoc);
		} 
		return lDocs;
	}

	int saveTestSite(LabelDoc testSite)
	{
		_db.LabelDocs.InsertOnSubmit(testSite);
		var recordsInserted = _db.GetChangeSet().Inserts.Count();
		_db.SubmitChanges();
		return recordsInserted;
	}

	IList<Label> getPreIdStudentsForSite(LabelDoc site, int studentCount)
	{
		var studentLabels = new List<Label>();
		var lastLabel = studentLabels.LastOrDefault();
		short pageNum = lastLabel?.PageNumber ?? 1;
		byte lblNum = lastLabel?.LabelNumber ?? 1;
		for(int i = 0; i < studentCount; i++ )
		{
			if (lblNum > labelsPerPage)
			{
				pageNum ++;
				lblNum = 1;
			}
			var lbl = new Label();
			lbl.PageNumber = pageNum;
			lbl.LabelNumber = lblNum;
			lbl.LabelDocId = site.LabelDocId;
			lbl.PreRegister = true;
			lbl.SiteCode = site.SiteCode.ToString();
			lbl.SiteName = site.SiteName;
			lbl.TestDateTime = site.TestDate.ToString();
			var first = this.getRandomFirstName();
			var last = this.getRandomLastName();
			lbl.StudentName = $"{last}, {first}";
			lbl.IseeId = this.getRandomInt(100000,999999).ToString();
			var grade = this.getRandomInt(2,4);
			lbl.GradeLevel = string.Format(gradeLevelFormatStr, grade);
			studentLabels.Add(lbl);
			lblNum++;
		}
		return studentLabels;
	}

	void appendGenericLabelsForSite(LabelDoc site, int studentCount, ref IList<Label> studentLabels)
	{
		var lastLabel = studentLabels.LastOrDefault();
		short pageNum = lastLabel?.PageNumber ?? 1;
		byte lblNum = lastLabel?.LabelNumber ?? 0;
		lblNum ++;
		for (int i = 0; i < studentCount; i++)
		{
			if (lblNum > labelsPerPage)
			{
				pageNum++;
				lblNum = 1;
			}
			var lbl = new Label();
			lbl.PageNumber = pageNum;
			lbl.LabelNumber = lblNum;
			lbl.LabelDocId = site.LabelDocId;
			lbl.PreRegister = false;
			lbl.SiteCode = site.SiteCode.ToString();
			lbl.SiteName = site.SiteName;
			lbl.TestDateTime = site.TestDate.ToString();
			lbl.StudentName = genericLabelStudentName;
			lbl.IseeId = this.getRandomInt(100000, 999999).ToString();
			var grade = this.getRandomInt(2, 4);
			lbl.GradeLevel = string.Format(gradeLevelFormatStr, grade);
			studentLabels.Add(lbl);
			lblNum++;
		}
	}

	int saveTestSiteStudents(ICollection<Label> students)
	{
		_db.Labels.InsertAllOnSubmit(students);
		var recordsInserted = _db.GetChangeSet().Inserts.Count();
		_db.SubmitChanges();
		return recordsInserted;
	}

	string slugEncode(string inString)
	{
		var sb = new StringBuilder();
		var invalidChars = Path.GetInvalidFileNameChars();
		foreach (var inStringChar in inString)
		{
			if (inStringChar == ' ' || invalidChars.Contains(inStringChar))
			{
				if (sb.ToString().ToCharArray().Last() == '-') continue;
				sb.Append("-");
			}
			else
			{
				sb.Append(inStringChar);
			}
		}
		return sb.ToString();
	}

}