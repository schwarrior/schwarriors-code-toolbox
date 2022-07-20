<Query Kind="Program" />

void Main()
{
	var connStr = "Data Source=DbServer;Initial Catalog=SampleData;User=SampleDataAppUser;Password=F2w2sWNR53UvTM;MultipleActiveResultSets=true;";
	var cleanConnStr = CleanConnectionString(connStr);
	cleanConnStr.Dump();
}

const string passwordSubstitute = "*******";

string CleanConnectionString(string connStr)
{
	var builder = new SqlConnectionStringBuilder(connStr);
	if (!string.IsNullOrWhiteSpace(builder.Password))
	{
		builder.Password = passwordSubstitute;
	}
	return builder.ToString();
}