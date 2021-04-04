<Query Kind="Program" />

void Main()
{
	//GetData();
	GetDataAsync();
}

async void GetDataAsync()
{
	"Csharp async ado net read db query result use using sqlconnection sqlcommand".Dump();
	using(var conn = new SqlConnection("Data Source=.;Initial Catalog=MyDatabase;Integrated Security=True;"))
	{
		conn.Open();
		using(var cmd = new SqlCommand("Select 1 union Select 2", conn))
		{
			using(var reader = await cmd.ExecuteReaderAsync())
			{
				while(reader.Read())
				{
					reader[0].Dump();
				}
			}
		}
	}
}

void GetData()
{
	"Csharp ado net read db query result use using sqlconnection sqlcommand".Dump();
	using(var conn = new SqlConnection("Data Source=.;Initial Catalog=MyDatabase;Integrated Security=True;"))
	{
		conn.Open();
		using(var cmd = new SqlCommand("Select 1 union Select 2", conn))
		{
			using(var reader = cmd.ExecuteReader())
			{
				while(reader.Read())
				{
					reader[0].Dump();
				}
			}
		}
	}
}