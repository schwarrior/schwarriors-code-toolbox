<Query Kind="Program" />

//this may not be the best way to do this
//http://stackoverflow.com/questions/7469405/how-can-i-list-all-registered-assemblies-in-gac
//gacutil -l >yourassemblies.txt

void Main()
{

	// List of all the different types of GAC folders for both 32bit and 64bit
// environments.
List<string> gacFolders = new List<string>() { 
    "GAC", "GAC_32", "GAC_64", "GAC_MSIL", 
    "NativeImages_v2.0.50727_32", 
    "NativeImages_v2.0.50727_64",
    "NativeImages_v4.0.30319_32",
    "NativeImages_v4.0.30319_64"
};

foreach (string folder in gacFolders)
{
    string path = Path.Combine(
       Environment.ExpandEnvironmentVariables(@"%systemroot%\assembly"), 
       folder);

    if(Directory.Exists(path))
    {
        ("<hr/>" + folder + "<hr/>").Dump();

        string[] assemblyFolders = Directory.GetDirectories(path);
        foreach (string assemblyFolder in assemblyFolders)
        {
			//(assemblyFolder + "<br/>").Dump();
			var subAssemblyFolders = Directory.GetDirectories(assemblyFolder);
			foreach (string subAssemblyFolder in subAssemblyFolders)
			{	
				(subAssemblyFolder + "<br/>").Dump();
			}
        }
    }
}
	
}

// Define other methods and classes here
