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
	const string gradeLevelFormatStr = "(PRIMARY{0})";

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

	string getRandomFirstName() 
	{
		var moveIndex = getRandomInt(0, firstNames.Count() - 1);
		return firstNames[moveIndex];
	}

	string getRandomLastName()
	{
		var moveIndex = getRandomInt(0, lastNames.Length - 1);
		return lastNames[moveIndex];
	}

	string getRandomSchoolName()
	{
		var moveIndex = getRandomInt(0, schools.Length - 1);
		return schools[moveIndex];
	}
	

	void checkClassConnections()
	{
		Console.WriteLine("Checking DB Connectivity from class");
		_db.LabelStocks.Take(numSampleTestSiteDocs).Dump();
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
		// var schools = _db.SampleData.Schools.Skip(skip).Take(siteCount).ToList();
		for (int i=0; i<siteCount; i++)
		{
			var SiteCode = getRandomInt().ToString();
			var SiteName = getRandomSchoolName();
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
		int pageNum = lastLabel?.PageNumber ?? 1;
		int lblNum = lastLabel?.LabelNumber ?? 1;
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
		int pageNum = lastLabel?.PageNumber ?? 1;
		int lblNum = lastLabel?.LabelNumber ?? 0;
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

	public static string SlugEncode(string inString)
	{
		var sb = new StringBuilder();
		var invalidChars = Path.GetInvalidFileNameChars();
		var slugOutChars = invalidChars + ":. ";
		foreach (var inStringChar in inString)
		{
			if (slugOutChars.Contains(inStringChar))
			{
				if(sb.ToString().ToCharArray().Last() == '-') continue;
				sb.Append("-");
			}
			else
			{
				sb.Append(inStringChar);
			}
		}
		return sb.ToString();
	}

	string[] lastNames = {
		"Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis", "Garcia", "Rodriguez", "Wilson", "Martinez", "Anderson", "Taylor", "Thomas", "Hernandez", "Moore", "Martin", "Jackson", "Thompson", "White", "Lopez", "Lee", "Gonzalez", "Harris", "Clark", "Lewis", "Robinson", "Walker", "Perez", "Hall", "Young", "Allen", "Sanchez", "Wright", "King", "Scott", "Green", "Baker", "Adams", "Nelson", "Hill", "Ramirez", "Campbell", "Mitchell", "Roberts", "Carter", "Phillips", "Evans", "Turner", "Torres", "Parker", "Collins", "Edwards", "Stewart", "Flores", "Morris", "Nguyen", "Murphy", "Rivera", "Cook", "Rogers", "Morgan", "Peterson", "Cooper", "Reed", "Bailey", "Bell", "Gomez", "Kelly", "Howard", "Ward", "Cox", "Diaz", "Richardson", "Wood", "Watson", "Brooks", "Bennett", "Gray", "James", "Reyes", "Cruz", "Hughes", "Price", "Myers", "Long", "Foster", "Sanders", "Ross", "Morales", "Powell", "Sullivan", "Russell", "Ortiz", "Jenkins", "Gutierrez", "Perry", "Butler", "Barnes", "Fisher", "Henderson", "Coleman", "Simmons", "Patterson", "Jordan", "Reynolds", "Hamilton", "Graham", "Kim", "Gonzales", "Alexander", "Ramos", "Wallace", "Griffin", "West", "Cole", "Hayes", "Chavez", "Gibson", "Bryant", "Ellis", "Stevens", "Murray", "Ford", "Marshall", "Owens", "Mcdonald", "Harrison", "Ruiz", "Kennedy", "Wells", "Alvarez", "Woods", "Mendoza", "Castillo", "Olson", "Webb", "Washington", "Tucker", "Freeman", "Burns", "Henry", "Vasquez", "Snyder", "Simpson", "Crawford", "Jimenez", "Porter", "Mason", "Shaw", "Gordon", "Wagner", "Hunter", "Romero", "Hicks", "Dixon", "Hunt", "Palmer", "Robertson", "Black", "Holmes", "Stone", "Meyer", "Boyd", "Mills", "Warren", "Fox", "Rose", "Rice", "Moreno", "Schmidt", "Patel", "Ferguson", "Nichols", "Herrera", "Medina", "Ryan", "Fernandez", "Weaver", "Daniels", "Stephens", "Gardner", "Payne", "Kelley", "Dunn", "Pierce", "Arnold", "Tran", "Spencer", "Peters", "Hawkins", "Grant", "Hansen", "Castro", "Hoffman", "Hart", "Elliott", "Cunningham", "Knight", "Bradley", "Carroll", "Hudson", "Duncan", "Armstrong", "Berry", "Andrews", "Johnston", "Ray", "Lane", "Riley", "Carpenter", "Perkins", "Aguilar", "Silva", "Richards", "Willis", "Matthews", "Chapman", "Lawrence", "Garza", "Vargas", "Watkins", "Wheeler", "Larson", "Carlson", "Harper", "George", "Greene", "Burke", "Guzman", "Morrison", "Munoz", "Jacobs", "Obrien", "Lawson", "Franklin", "Lynch", "Bishop", "Carr", "Salazar", "Austin", "Mendez", "Gilbert", "Jensen", "Williamson", "Montgomery", "Harvey", "Oliver", "Howell", "Dean", "Hanson", "Weber", "Garrett", "Sims", "Burton", "Fuller", "Soto", "Mccoy", "Welch", "Chen", "Schultz", "Walters", "Reid", "Fields", "Walsh", "Little", "Fowler", "Bowman", "Davidson", "May", "Day", "Schneider", "Newman", "Brewer", "Lucas", "Holland", "Wong", "Banks", "Santos", "Curtis", "Pearson", "Delgado", "Valdez", "Pena", "Rios", "Douglas", "Sandoval", "Barrett", "Hopkins", "Keller", "Guerrero", "Stanley", "Bates", "Alvarado", "Beck", "Ortega", "Wade", "Estrada", "Contreras", "Barnett", "Caldwell", "Santiago", "Lambert", "Powers", "Chambers", "Nunez", "Craig", "Leonard", "Lowe", "Rhodes", "Byrd", "Gregory", "Shelton", "Frazier", "Becker", "Maldonado", "Fleming", "Vega", "Sutton", "Cohen", "Jennings", "Parks", "Mcdaniel", "Watts", "Barker", "Norris", "Vaughn", "Vazquez", "Holt", "Schwartz", "Steele", "Benson", "Neal", "Dominguez", "Horton", "Terry", "Wolfe", "Hale", "Lyons", "Graves", "Haynes", "Miles", "Park", "Warner", "Padilla", "Bush", "Thornton", "Mccarthy", "Mann", "Zimmerman", "Erickson", "Fletcher", "Mckinney", "Page", "Dawson", "Joseph", "Marquez", "Reeves", "Klein", "Espinoza", "Baldwin", "Moran", "Love", "Robbins", "Higgins", "Ball", "Cortez", "Le", "Griffith", "Bowen", "Sharp", "Cummings", "Ramsey", "Hardy", "Swanson", "Barber", "Acosta", "Luna", "Chandler", "Blair", "Daniel", "Cross", "Simon", "Dennis", "Oconnor", "Quinn", "Gross", "Navarro", "Moss", "Fitzgerald", "Doyle", "Mclaughlin", "Rojas", "Rodgers", "Stevenson", "Singh", "Yang", "Figueroa", "Harmon", "Newton", "Paul", "Manning", "Garner", "Mcgee", "Reese", "Francis", "Burgess", "Adkins", "Goodman", "Curry", "Brady", "Christensen", "Potter", "Walton", "Goodwin", "Mullins", "Molina", "Webster", "Fischer", "Campos", "Avila", "Sherman", "Todd", "Chang", "Blake", "Malone", "Wolf", "Hodges", "Juarez", "Gill", "Farmer", "Hines", "Gallagher", "Duran", "Hubbard", "Cannon", "Miranda", "Wang", "Saunders", "Tate", "Mack", "Hammond", "Carrillo", "Townsend", "Wise", "Ingram", "Barton", "Mejia", "Ayala", "Schroeder", "Hampton", "Rowe", "Parsons", "Frank", "Waters", "Strickland", "Osborne", "Maxwell", "Chan", "Deleon", "Norman", "Harrington", "Casey", "Patton", "Logan", "Bowers", "Mueller", "Glover", "Floyd", "Hartman", "Buchanan", "Cobb", "French", "Kramer", "Mccormick", "Clarke", "Tyler", "Gibbs", "Moody", "Conner", "Sparks", "Mcguire", "Leon", "Bauer", "Norton", "Pope", "Flynn", "Hogan", "Robles", "Salinas", "Yates", "Lindsey", "Lloyd", "Marsh", "Mcbride", "Owen", "Solis", "Pham", "Lang", "Pratt", "Lara", "Brock", "Ballard", "Trujillo", "Shaffer", "Drake", "Roman", "Aguirre", "Morton", "Stokes", "Lamb", "Pacheco", "Patrick", "Cochran", "Shepherd", "Cain", "Burnett", "Hess", "Li", "Cervantes", "Olsen", "Briggs", "Ochoa", "Cabrera", "Velasquez", "Montoya", "Roth", "Meyers", "Cardenas", "Fuentes", "Weiss", "Hoover", "Wilkins", "Nicholson", "Underwood", "Short", "Carson", "Morrow", "Colon", "Holloway", "Summers", "Bryan", "Petersen", "Mckenzie", "Serrano", "Wilcox", "Carey", "Clayton", "Poole", "Calderon", "Gallegos", "Greer", "Rivas", "Guerra", "Decker", "Collier", "Wall", "Whitaker", "Bass", "Flowers", "Davenport", "Conley", "Houston", "Huff", "Copeland", "Hood", "Monroe", "Massey", "Roberson", "Combs", "Franco", "Larsen", "Pittman", "Randall", "Skinner", "Wilkinson", "Kirby", "Cameron", "Bridges", "Anthony", "Richard", "Kirk", "Bruce", "Singleton", "Mathis", "Bradford", "Boone", "Abbott", "Charles", "Allison", "Sweeney", "Atkinson", "Horn", "Jefferson", "Rosales", "York", "Christian", "Phelps", "Farrell", "Castaneda", "Nash", "Dickerson", "Bond", "Wyatt", "Foley", "Chase", "Gates", "Vincent", "Mathews", "Hodge", "Garrison", "Trevino", "Villarreal", "Heath", "Dalton", "Valencia", "Callahan", "Hensley", "Atkins", "Huffman", "Roy", "Boyer", "Shields", "Lin", "Hancock", "Grimes", "Glenn", "Cline", "Delacruz", "Camacho", "Dillon", "Parrish", "Oneill", "Melton", "Booth", "Kane", "Berg", "Harrell", "Pitts", "Savage", "Wiggins", "Brennan", "Salas", "Marks", "Russo", "Sawyer", "Baxter", "Golden", "Hutchinson", "Liu", "Walter", "Mcdowell", "Wiley", "Rich", "Humphrey", "Johns", "Koch", "Suarez", "Hobbs", "Beard", "Gilmore", "Ibarra", "Keith", "Macias", "Khan", "Andrade", "Ware", "Stephenson", "Henson", "Wilkerson", "Dyer", "Mcclure", "Blackwell", "Mercado", "Tanner", "Eaton", "Clay", "Barron", "Beasley", "Oneal", "Preston", "Small", "Wu", "Zamora", "Macdonald", "Vance", "Snow", "Mcclain", "Stafford", "Orozco", "Barry", "English", "Shannon", "Kline", "Jacobson", "Woodard", "Huang", "Kemp", "Mosley", "Prince", "Merritt", "Hurst", "Villanueva", "Roach", "Nolan", "Lam", "Yoder", "Mccullough", "Lester", "Santana", "Valenzuela", "Winters", "Barrera", "Leach", "Orr", "Berger", "Mckee", "Strong", "Conway", "Stein", "Whitehead", "Bullock", "Escobar", "Knox", "Meadows", "Solomon", "Velez", "Odonnell", "Kerr", "Stout", "Blankenship", "Browning", "Kent", "Lozano", "Bartlett", "Pruitt", "Buck", "Barr", "Gaines", "Durham", "Gentry", "Mcintyre", "Sloan", "Melendez", "Rocha", "Herman", "Sexton", "Moon", "Hendricks", "Rangel", "Stark", "Lowery", "Hardin", "Hull", "Sellers", "Ellison", "Calhoun", "Gillespie", "Mora", "Knapp", "Mccall", "Morse", "Dorsey", "Weeks", "Nielsen", "Livingston", "Leblanc", "Mclean", "Bradshaw", "Glass", "Middleton", "Buckley", "Schaefer", "Frost", "Howe", "House", "Mcintosh", "Ho", "Pennington", "Reilly", "Hebert", "Mcfarland", "Hickman", "Noble", "Spears", "Conrad", "Arias", "Galvan", "Velazquez", "Huynh", "Frederick", "Randolph", "Cantu", "Fitzpatrick", "Mahoney", "Peck", "Villa", "Michael", "Donovan", "Mcconnell", "Walls", "Boyle", "Mayer", "Zuniga", "Giles", "Pineda", "Pace", "Hurley", "Mays", "Mcmillan", "Crosby", "Ayers", "Case", "Bentley", "Shepard", "Everett", "Pugh", "David", "Mcmahon", "Dunlap", "Bender", "Hahn", "Harding", "Acevedo", "Raymond", "Blackburn", "Duffy", "Landry", "Dougherty", "Bautista", "Shah", "Potts", "Arroyo", "Valentine", "Meza", "Gould", "Vaughan", "Fry", "Rush", "Avery", "Herring", "Dodson", "Clements", "Sampson", "Tapia", "Bean", "Lynn", "Crane", "Farley", "Cisneros", "Benton", "Ashley", "Mckay", "Finley", "Best", "Blevins", "Friedman", "Moses", "Sosa", "Blanchard", "Huber", "Frye", "Krueger", "Bernard", "Rosario", "Rubio", "Mullen", "Benjamin", "Haley", "Chung", "Moyer", "Choi", "Horne", "Yu", "Woodward", "Ali", "Nixon", "Hayden", "Rivers", "Estes", "Mccarty", "Richmond", "Stuart", "Maynard", "Brandt", "Oconnell", "Hanna", "Sanford", "Sheppard", "Church", "Burch", "Levy", "Rasmussen", "Coffey", "Ponce", "Faulkner", "Donaldson", "Schmitt", "Novak", "Costa", "Montes", "Booker", "Cordova", "Waller", "Arellano", "Maddox", "Mata", "Bonilla", "Stanton", "Compton", "Kaufman", "Dudley", "Mcpherson", "Beltran", "Dickson", "Mccann", "Villegas", "Proctor", "Hester", "Cantrell", "Daugherty", "Cherry", "Bray", "Davila", "Rowland", "Levine", "Madden", "Spence", "Good", "Irwin", "Werner", "Krause", "Petty", "Whitney", "Baird", "Hooper", "Pollard", "Zavala", "Jarvis", "Holden", "Haas", "Hendrix", "Mcgrath", "Bird", "Lucero", "Terrell", "Riggs", "Joyce", "Mercer", "Rollins", "Galloway", "Duke", "Odom", "Andersen", "Downs", "Hatfield", "Benitez", "Archer", "Huerta", "Travis", "Mcneil", "Hinton", "Zhang", "Hays", "Mayo", "Fritz", "Branch", "Mooney", "Ewing", "Ritter", "Esparza", "Frey", "Braun", "Gay", "Riddle", "Haney", "Kaiser", "Holder", "Chaney", "Mcknight", "Gamble", "Vang", "Cooley", "Carney", "Cowan", "Forbes", "Ferrell", "Davies", "Barajas", "Shea", "Osborn", "Bright", "Cuevas", "Bolton", "Murillo", "Lutz", "Duarte", "Kidd", "Key", "Cooke"
	};

	string[] firstNames = {
		"James", "John", "Robert", "Michael", "William", "David", "Richard", "Charles", "Joseph", "Thomas", "Christopher", "Daniel", "Paul", "Mark", "Donald", "George", "Kenneth", "Steven", "Edward", "Brian", "Ronald", "Anthony", "Kevin", "Jason", "Matthew", "Gary", "Timothy", "Jose", "Larry", "Jeffrey", "Frank", "Scott", "Eric", "Stephen", "Andrew", "Raymond", "Gregory", "Joshua", "Jerry", "Dennis", "Walter", "Patrick", "Peter", "Harold", "Douglas", "Henry", "Carl", "Arthur", "Ryan", "Roger", "Joe", "Juan", "Jack", "Albert", "Jonathan", "Justin", "Terry", "Gerald", "Keith", "Samuel", "Willie", "Ralph", "Lawrence", "Nicholas", "Roy", "Benjamin", "Bruce", "Brandon", "Adam", "Harry", "Fred", "Wayne", "Billy", "Steve", "Louis", "Jeremy", "Aaron", "Randy", "Howard", "Eugene", "Carlos", "Russell", "Bobby", "Victor", "Martin", "Ernest", "Phillip", "Todd", "Jesse", "Craig", "Alan", "Shawn", "Clarence", "Sean", "Philip", "Chris", "Johnny", "Earl", "Jimmy", "Antonio", "Danny", "Bryan", "Tony", "Luis", "Mike", "Stanley", "Leonard", "Nathan", "Dale", "Manuel", "Rodney", "Curtis", "Norman", "Allen", "Marvin", "Vincent", "Glenn", "Jeffery", "Travis", "Jeff", "Chad", "Jacob", "Lee", "Melvin", "Alfred", "Kyle", "Francis", "Bradley", "Jesus", "Herbert", "Frederick", "Ray", "Joel", "Edwin", "Don", "Eddie", "Ricky", "Troy", "Randall", "Barry", "Alexander", "Bernard", "Mario", "Leroy", "Francisco", "Marcus", "Micheal", "Theodore", "Clifford", "Miguel", "Oscar", "Jay", "Jim", "Tom", "Calvin", "Alex", "Jon", "Ronnie", "Bill", "Lloyd", "Tommy", "Leon", "Derek", "Warren", "Darrell", "Jerome", "Floyd", "Leo", "Alvin", "Tim", "Wesley", "Gordon", "Dean", "Greg", "Jorge", "Dustin", "Pedro", "Derrick", "Dan", "Lewis", "Zachary", "Corey", "Herman", "Maurice", "Vernon", "Roberto", "Clyde", "Glen", "Hector", "Shane", "Ricardo", "Sam", "Rick", "Lester", "Brent", "Ramon", "Charlie", "Tyler", "Gilbert", "Gene", "Marc", "Reginald", "Ruben", "Brett", "Angel", "Nathaniel", "Rafael", "Leslie", "Edgar", "Milton", "Raul", "Ben", "Chester", "Cecil", "Duane", "Franklin", "Andre", "Elmer", "Brad", "Gabriel", "Ron", "Mitchell", "Roland", "Arnold", "Harvey", "Jared", "Adrian", "Karl", "Cory", "Claude", "Erik", "Darryl", "Jamie", "Neil", "Jessie", "Christian", "Javier", "Fernando", "Clinton", "Ted", "Mathew", "Tyrone", "Darren", "Lonnie", "Lance", "Cody", "Julio", "Kelly", "Kurt", "Allan", "Nelson", "Guy", "Clayton", "Hugh", "Max", "Dwayne", "Dwight", "Armando", "Felix", "Jimmie", "Everett", "Jordan", "Ian", "Wallace", "Ken", "Bob", "Jaime", "Casey", "Alfredo", "Alberto", "Dave", "Ivan", "Johnnie", "Sidney", "Byron", "Julian", "Isaac", "Morris", "Clifton", "Willard", "Daryl", "Ross", "Virgil", "Andy", "Marshall", "Salvador", "Perry", "Kirk", "Sergio", "Marion", "Tracy", "Seth", "Kent", "Terrance", "Rene", "Eduardo", "Terrence", "Enrique", "Freddie", "Wade", "Mary", // begin girls
		"Patricia", "Linda", "Barbara", "Elizabeth", "Jennifer", "Maria", "Susan", "Margaret", "Dorothy", "Lisa", "Nancy", "Karen", "Betty", "Helen", "Sandra", "Donna", "Carol", "Ruth", "Sharon", "Michelle", "Laura", "Sarah", "Kimberly", "Deborah", "Jessica", "Shirley", "Cynthia", "Angela", "Melissa", "Brenda", "Amy", "Anna", "Rebecca", "Virginia", "Kathleen", "Pamela", "Martha", "Debra", "Amanda", "Stephanie", "Carolyn", "Christine", "Marie", "Janet", "Catherine", "Frances", "Ann", "Joyce", "Diane", "Alice", "Julie", "Heather", "Teresa", "Doris", "Gloria", "Evelyn", "Jean", "Cheryl", "Mildred", "Katherine", "Joan", "Ashley", "Judith", "Rose", "Janice", "Kelly", "Nicole", "Judy", "Christina", "Kathy", "Theresa", "Beverly", "Denise", "Tammy", "Irene", "Jane", "Lori", "Rachel", "Marilyn", "Andrea", "Kathryn", "Louise", "Sara", "Anne", "Jacqueline", "Wanda", "Bonnie", "Julia", "Ruby", "Lois", "Tina", "Phyllis", "Norma", "Paula", "Diana", "Annie", "Lillian", "Emily", "Robin", "Peggy", "Crystal", "Gladys", "Rita", "Dawn", "Connie", "Florence", "Tracy", "Edna", "Tiffany", "Carmen", "Rosa", "Cindy", "Grace", "Wendy", "Victoria", "Edith", "Kim", "Sherry", "Sylvia", "Josephine", "Thelma", "Shannon", "Sheila", "Ethel", "Ellen", "Elaine", "Marjorie", "Carrie", "Charlotte", "Monica", "Esther", "Pauline", "Emma", "Juanita", "Anita", "Rhonda", "Hazel", "Amber", "Eva", "Debbie", "April", "Leslie", "Clara", "Lucille", "Jamie", "Joanne", "Eleanor", "Valerie", "Danielle", "Megan", "Alicia", "Suzanne", "Michele", "Gail", "Bertha", "Darlene", "Veronica", "Jill", "Erin", "Geraldine", "Lauren", "Cathy", "Joann", "Lorraine", "Lynn", "Sally", "Regina", "Erica", "Beatrice", "Dolores", "Bernice", "Audrey", "Yvonne", "Annette", "June", "Samantha", "Marion", "Dana", "Stacy", "Ana", "Renee", "Ida", "Vivian", "Roberta", "Holly", "Brittany", "Melanie", "Loretta", "Yolanda", "Jeanette", "Laurie", "Katie", "Kristen", "Vanessa", "Alma", "Sue", "Elsie", "Beth", "Jeanne", "Vicki", "Carla", "Tara", "Rosemary", "Eileen", "Terri", "Gertrude", "Lucy", "Tonya", "Ella", "Stacey", "Wilma", "Gina", "Kristin", "Jessie", "Natalie", "Agnes", "Vera", "Willie", "Charlene", "Bessie", "Delores", "Melinda", "Pearl", "Arlene", "Maureen", "Colleen", "Allison", "Tamara", "Joy", "Georgia", "Constance", "Lillie", "Claudia", "Jackie", "Marcia", "Tanya", "Nellie", "Minnie", "Marlene", "Heidi", "Glenda", "Lydia", "Viola", "Courtney", "Marian", "Stella", "Caroline", "Dora", "Jo", "Vickie", "Mattie", "Terry", "Maxine", "Irma", "Mabel", "Marsha", "Myrtle", "Lena", "Christy", "Deanna", "Patsy", "Hilda", "Gwendolyn", "Jennie", "Nora", "Margie", "Nina", "Cassandra", "Leah", "Penny", "Kay", "Priscilla", "Naomi", "Carole", "Brandy", "Olga", "Billie", "Dianne", "Tracey", "Leona", "Jenny", "Felicia", "Sonia", "Miriam", "Velma", "Becky", "Bobbie", "Violet", "Kristina", "Toni", "Misty", "Mae", "Shelly", "Daisy", "Ramona", "Sherri", "Erika", "Katrina", "Claire"
	};

	string[] schools = {
		"George Washington Elementary", "John Adams Elementary", "Thomas Jefferson Elementary", "James Madison Elementary", "James Monroe Elementary", "John Quincy Adams Elementary", "Andrew Jackson Elementary", "Martin Van Buren Elementary", "William Henry Harrison Elementary", "John Tyler Elementary", "James K. Polk Elementary", "Zachary Taylor Elementary", "Millard Fillmore Elementary", "Franklin Pierce Elementary", "James Buchanan Elementary", "Abraham Lincoln Elementary", "Andrew Johnson Elementary", "Ulysses S. Grant Elementary", "Rutherford B. Hayes Elementary", "James A. Garfield Elementary", "Chester A. Arthur Elementary", "Grover Cleveland Elementary", "Benjamin Harrison Elementary", "Grover Cleveland Elementary", "William McKinley Elementary", "Theodore Roosevelt Elementary", "William Howard Taft Elementary", "Woodrow Wilson Elementary", "Warren G. Harding Elementary", "Calvin Coolidge Elementary", "Herbert Hoover Elementary", "Franklin D. Roosevelt Elementary", "Harry S. Truman Elementary", "Dwight D. Eisenhower Elementary", "John F. Kennedy Elementary", "Lyndon B. Johnson Elementary", "Richard Nixon Elementary", "Gerald Ford Elementary", "Jimmy Carter Elementary", "Ronald Reagan Elementary", "George H. W. Bush Elementary", "Bill Clinton Elementary", "George W. Bush Elementary"
	};


}