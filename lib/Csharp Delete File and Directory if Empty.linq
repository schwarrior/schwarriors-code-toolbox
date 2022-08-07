<Query Kind="Program">
  <Namespace>System</Namespace>
  <Namespace>System.IO</Namespace>
  <Namespace>System.Threading</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	string[] filesToDelete = 
	{ 
		@"C:\temp\DelTest\DirA\File1.txt", 
		@"C:\Temp\DelTest\DirB\File2.txt", 
		@"C:\Temp\DelTest\DirC\File3.txt" 
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
//using System;
//using System.IO;
//using System.Threading;
//using System.Threading.Tasks;

public class FileSystemGateway
{

	public static string SaveBytesToFile (byte[] fileBytes, string saveFileFullPath)
	{
		try
		{
			var byteSize = fileBytes.Length;
			File.WriteAllBytes(saveFileFullPath, fileBytes);
			return String.Empty;
		}
		catch (Exception ex)
		{
			return $"Could not save '{saveFileFullPath}'. {ex.Message}";
		}
	}

	public static string DeleteFileAndParentDirIfEmpty(string fileFullPath, out bool fileDeleted, out bool dirDeleted)
	{
		dirDeleted = false;
		var deleteFileError = DeleteFile(fileFullPath, out fileDeleted);
		if ( !fileDeleted || !string.IsNullOrEmpty(deleteFileError) ) return deleteFileError;
		var dirPath = Path.GetDirectoryName(fileFullPath);
		var dirIsEmpty = DirIsEmptyOrOnlyLogs(dirPath);
		if (!dirIsEmpty) return String.Empty;
		var deleteFolderError = DeleteDir(dirPath, out dirDeleted);
		if (!string.IsNullOrEmpty(deleteFolderError)) return deleteFolderError;
		return string.Empty;
	}

	public static string DeleteFile(string filePath, out bool fileDeleted)
	{
		fileDeleted = false;
		try
		{
			if (!File.Exists(filePath))
			{
				return $"Could not delete file. File not found '{filePath}'";
			}
			File.Delete(filePath);
			fileDeleted = true;
			return string.Empty;
		}
		catch (Exception ex)
		{
			return $"Could not delete file '{filePath}'. {ex.Message}";
		}
	}

	public static string DeleteDir(string dirPath, out bool dirDeleted)
	{
		dirDeleted = false;
		try
		{
			if (!Directory.Exists(dirPath))
			{
				return $"Could not delete folder. Folder not found '{dirPath}'";
			}
			Directory.Delete(dirPath, true);
			dirDeleted = true;
			return String.Empty;
		}
		catch (Exception ex)
		{
			return $"Could not delete folder '{dirPath}'. {ex.Message}";
		}
	}

	public static bool DirIsEmptyOrOnlyLogs(string dirPath)
	{
		if (!Directory.Exists(dirPath)) return false;
		var directoryFiles = Directory.GetFiles(dirPath);
		if (directoryFiles.Length == 0) return true;
		var dirLogFiles = Directory.GetFiles(dirPath, "*.log");
		if (dirLogFiles.Length == 0) return true;
		if (directoryFiles.Length == dirLogFiles.Length) return true;
		return false;
	}

}
