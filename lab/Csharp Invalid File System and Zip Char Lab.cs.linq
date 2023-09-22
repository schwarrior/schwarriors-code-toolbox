<Query Kind="Statements" />

// Windows file system invalid character lab
// Exploring invalid character collections build

Console.WriteLine(System.IO.Path.InvalidPathChars);
Console.WriteLine();

// Received warning 'Please use GetInvalidPathChars or GetInvalidFileNameChars instead'

Console.WriteLine(System.IO.Path.GetInvalidFileNameChars());
Console.WriteLine(System.IO.Path.GetInvalidPathChars());
Console.WriteLine();

// Unfortately none of above declare comma invalid. Also checked for char as int 44 or hex 0x2C
// Perhaps the invalid comma issue is unique to zip archives?

// Solution 1: append comma to invalid chars

var invalidInternalZipFileNameChars = System.IO.Path.GetInvalidFileNameChars().Append(',');
Console.WriteLine(invalidInternalZipFileNameChars);

// Solution 2: use a white-list based replacement. Replace every character that is not strictly alphanumeric with a dash
// https://github.com/schwarrior/schwarriors-code-toolbox/blob/main/lib/Csharp%20Slug%20Encode%20Remove%20Incompatible%20Path%20Unc%20and%20Url%20Uri%20Characters.cs.linq