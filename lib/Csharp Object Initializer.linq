<Query Kind="Program" />

void Main()
{
	var connections = new List<ConnectionInfo> 
	{
		new ConnectionInfo {
			ConnectionName = "Name",
			ConnectionString = "String"
		}
	};
	connections.Dump();
	var connection = connections.First (c => c.ConnectionName == "Name");
	connection.Dump();
}

// Define other methods and classes here
public class ConnectionInfo
{
	public string ConnectionName { get; set; }
	public string ConnectionString { get; set; }
	public bool? Validated { get; set; }
}