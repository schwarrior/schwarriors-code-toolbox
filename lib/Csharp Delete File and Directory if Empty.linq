<Query Kind="Program">
  <Namespace>System.IO</Namespace>
</Query>

void Main()
{
	var testRoot = @"C:\Dev\Temp";
	var testFolder = "DelTest";
	FileSystemGateway.SetupTestFiles(testRoot, testFolder);
	
	string[] filesToDelete = 
	{ 
		$"{testRoot}\\{testFolder}\\DirA\\File1.txt", 
		$"{testRoot}\\{testFolder}\\DirB\\File2.txt", 
		$"{testRoot}\\{testFolder}\\DirC\\File3.txt",
		$"{testRoot}\\{testFolder}\\DirD\\File4.txt",
	};
	
	foreach (var fileToDelete in filesToDelete)
	{
		bool fileDeleted = false;
		bool dirDeleted = false;
		var delError = FileSystemGateway.DeleteFileAndParentDirIfEmpty
			(fileToDelete, out fileDeleted, out dirDeleted);
		if (!string.IsNullOrEmpty(delError))
		{
			Console.WriteLine ("An error occurred while attempting to delete a file then its parent folder. " +
				$"File path: '{fileToDelete}'. File reported as deleted: {fileDeleted}. " +
				$"Folder reported as deleted: {dirDeleted}. Error: {delError}");
		} 
		else
		{
			Console.WriteLine("Attempted to delete a file then its parent folder. " +
				$"File path: '{fileToDelete}'. File reported as deleted: {fileDeleted}. " +
				$"Folder reported as deleted: {dirDeleted}.");
		}
	}
	
}


//usings copied into linqPad program properties:
//using System.IO;

public class FileSystemGateway
{

	public static void SetupTestFiles(string testRoot, string testFolder)
	{
		Directory.Delete($"{testRoot}\\{testFolder}", true);
		Directory.CreateDirectory($"{testRoot}\\{testFolder}");
		Directory.CreateDirectory($"{testRoot}\\{testFolder}\\DirA");
		var f1 = File.CreateText($"{testRoot}\\{testFolder}\\DirA\\File1.txt");
		f1.Close();
		Directory.CreateDirectory($"{testRoot}\\{testFolder}\\DirB");
		var f2 = File.CreateText($"{testRoot}\\{testFolder}\\DirB\\File2.txt");
		f2.Close();
		var l = File.CreateText($"{testRoot}\\{testFolder}\\DirB\\DelTest.log");
		l.Close();
		Directory.CreateDirectory($"{testRoot}\\{testFolder}\\DirC");
		Directory.CreateDirectory($"{testRoot}\\{testFolder}\\DirD");
		var f0 = File.CreateText($"{testRoot}\\{testFolder}\\DirD\\File0.txt");
		f0.Close();
	}

	public static string DeleteFileAndParentDirIfEmpty(string fileFullPath, out bool fileDeleted, out bool dirDeleted)
	{
		dirDeleted = false;
		var deleteFileError = DeleteFile(fileFullPath, out fileDeleted);
		var dirPath = Path.GetDirectoryName(fileFullPath);
		var dirNotFound = !Directory.Exists(dirPath);
		var dirNotEmpty = !DirExistsAndIsEmptyOrOnlyLogs(dirPath);
		var deleteFolderError = string.Empty;
		if (dirNotFound)
		{
			deleteFolderError = $"Could not delete folder. Folder not found '{dirPath}'. ";	
		}
		else if (!dirNotFound && dirNotEmpty)
		{
			deleteFolderError = $"Could not delete folder. Folder has at least one child item that is not a log file '{dirPath}'. ";
		}
		else
		{
			deleteFolderError = DeleteDir(dirPath, out dirDeleted);
		}
		var combinedError = deleteFileError + deleteFolderError;
		return combinedError;
	}

	public static string DeleteFile(string filePath, out bool fileDeleted)
	{
		fileDeleted = false;
		try
		{
			if (!File.Exists(filePath))
			{
				return $"Could not delete file. File not found '{filePath}'. ";
			}
			File.Delete(filePath);
			fileDeleted = true;
			return string.Empty;
		}
		catch (Exception ex)
		{
			return $"Could not delete file '{filePath}'. {ex.Message}. ";
		}
	}

	public static string DeleteDir(string dirPath, out bool dirDeleted)
	{
		dirDeleted = false;
		try
		{
			if (!Directory.Exists(dirPath))
			{
				return $"Could not delete folder. Folder not found '{dirPath}'. ";
			}
			Directory.Delete(dirPath, true);
			dirDeleted = true;
			return String.Empty;
		}
		catch (Exception ex)
		{
			return $"Could not delete folder '{dirPath}'. {ex.Message}. ";
		}
	}

	public static bool DirExistsAndIsEmptyOrOnlyLogs(string dirPath)
	{
		if (!Directory.Exists(dirPath)) return false;
		var subDirs = Directory.GetDirectories(dirPath);
		if (subDirs.Length > 0) return false;
		var directoryFiles = Directory.GetFiles(dirPath);
		if (directoryFiles.Length == 0) return true;
		var dirLogFiles = Directory.GetFiles(dirPath, "*.log");
		if (directoryFiles.Length == dirLogFiles.Length) return true;
		return false;
	}

}