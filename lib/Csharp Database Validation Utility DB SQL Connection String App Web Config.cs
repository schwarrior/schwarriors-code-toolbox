public class DatabaseValidationManager
    {
        public static bool ValidateDatabaseConnection(string databaseConnectionName, out string validationResultMessage)
        {
            validationResultMessage = "Database connection validity unknown";
            bool isValid;
            var server = "Unknown";
            var dbName = "Unknown";
            try
            {
                var databaseConnectionString = GetDatabaseConnection(databaseConnectionName);
                if(string.IsNullOrWhiteSpace(databaseConnectionString)) throw new Exception(string.Format("No connection string found for connection {0}", databaseConnectionName));
                var connectionBuilder = new SqlConnectionStringBuilder(databaseConnectionString);
                server = connectionBuilder.DataSource;
                dbName = connectionBuilder.InitialCatalog;
                using (var testConn = new SqlConnection(databaseConnectionString))
                {
                    testConn.Open();
                    using (var testCmd = new SqlCommand("Select 1", testConn))
                    {
                        var testCmdResult = testCmd.ExecuteScalar();
                        var testCmdResultInt = testCmdResult as int?;
                        if (!testCmdResultInt.HasValue || testCmdResultInt != 1)
                            throw new Exception(string.Format("Test command expected result 1 but got {0}", testCmdResult));
                    }
                }
                validationResultMessage = string.Format("Database connection validated. Name {0}. Server {1}. Database {2}",
                    databaseConnectionName, server, dbName);
                isValid = true;
            }
            catch (Exception exception)
            {
                validationResultMessage = string.Format("Could not connect to connection name {0} on server {1} in database {2}. Error Detail: {3}",
                    databaseConnectionName, server, dbName, exception.Message);
                isValid = false;
            }
            return isValid;
        }

        private static string GetDatabaseConnection(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
	
	class Program
    {
        private static List<string> _expectedConnectionStringNames = new List<string>
        {
            "MyConn1",
            "MyConn2",
            "MyConn3"
        };

        static void Main()
        {
            Console.WriteLine("App Starting");
            Console.WriteLine();

            if (ValidateDatabaseConnections())
            {
                //do stuff

                Console.WriteLine();
                Console.WriteLine("App Complete");    
            }

            if (!Properties.Settings.Default.InteractiveMode) return;

            Console.WriteLine();
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }

        static bool ValidateDatabaseConnections()
        {
            var allConnsValid = true;
            _expectedConnectionStringNames.ForEach(connName =>
            {
                string connValidationMessage;
                var connIsValid = DatabaseValidationManager.ValidateDatabaseConnection(connName,
                    out connValidationMessage);
                Console.WriteLine(connValidationMessage);
                allConnsValid = allConnsValid && connIsValid;
            });
            Console.WriteLine();
            return allConnsValid;
        }
    }