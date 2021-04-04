using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SampleProgram
{
    class Program
    {

        /// <summary>
        /// SampleProgram [dbConnectionString(required)] [--mockMode(optional)] [--interactive(optional)]
        /// </summary>
        /// <param name="args">
        /// example:
        /// SampleProgram "Data Source=.;Initial Catalog=SampleData;Persist Security Info=True;Integrated Security=True;" --mockMode
        /// </param>
        /// <returns>
        /// 0 success
        /// 1 no args provided
        /// 2 arg found but is invalid connection string
        /// 3 errors while running program
        /// </returns>
        static int Main(string[] args)
        {
            Console.WriteLine("Sample Program Starting ...");
            
            Console.WriteLine(getExeVersion());

            var connectionString = string.Empty;
            var mockMode = false;
            var interactive = false;

            foreach (var arg in args)
            {
                var argLower = arg.ToLower();
                switch (argLower)
                {
                    case "--mockmode":
                    case "-mockmode":
                    case "--m":
                    case "-m":
                        mockMode = true;
                        break;
                    case "--interactive":
                    case "-interactive":
                    case "--i":
                    case "-i":
                        interactive = true;
                        break;
                    default:
                        connectionString = arg;
                        break;
                }
            }
            
            //validate conn string part 1
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("The SampleProgram requires a connection string to be passed as command line parameter.");
                Console.WriteLine("No arguments found or connections string is blank.");
                Console.WriteLine("Program will exit.");
                return 1;
            }

            //validate conn string part 2
            try
            {
                var csb = new DbConnectionStringBuilder();
                csb.ConnectionString = connectionString; // throws
            }
            catch (Exception)
            {
                Console.WriteLine("The SampleProgram requires a valid connection string to be passed as command line parameter.");
                Console.WriteLine("Program will exit.");
                return 2;
            }

            Console.WriteLine("Connection string found.");
            Console.WriteLine(connectionString);
            Console.WriteLine("Running in {0}interactive mode.", interactive?string.Empty:"non-");
            Console.WriteLine(mockMode ? "Running in mock mode." : "Running in live mode.");

            var result = "Sample Program Result";
            // result = new SampleProgram(connectionString, mockMode).DoWork();
            
            Console.WriteLine(result);

            Console.WriteLine("Sample Program Done.");

            if (interactive)
            {
                Console.WriteLine("Press Enter to Close.");
                Console.ReadLine();
            }

            return (string.IsNullOrWhiteSpace(result) ? 0 : 3);
        }

        static string getExeVersion()
        {
            var exeAssembly = Assembly.GetEntryAssembly();
            var exeName = exeAssembly.GetName();
            var exeVersion = exeName.Version;
            return exeVersion.ToString(4);
        }
    }
}
