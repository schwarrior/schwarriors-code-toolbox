<Query Kind="Statements" />

const string oldFilePath = @"c:\Users\Public\Documents\FileA.txt";
const string newFileName = "FileB.txt";
var directory = Path.GetDirectoryName(oldFilePath);
var newFilePath = Path.Combine(directory, newFileName);
//File.Move(oldFilePath, newFilePath);
Console.WriteLine("Moved from {0} to {1}", oldFilePath, newFilePath);