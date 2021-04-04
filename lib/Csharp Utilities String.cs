public class StringConversion
{

    public static string StringToEllipsisLimitedString(this string fullString, int characterLimit, string ellipsisText = "...")
    {
        if (fullString.Length > characterLimit)
            return fullString.Substring(0, characterLimit - ellipsisText.Length) + ellipsisText;
        return fullString;
    }

    public static string CamelCaseStringToSentenceString(string camelCaseString)
    {
        if (camelCaseString.Length <= 4) return camelCaseString;
        var sb = new StringBuilder();
        var chars = new List<char>(camelCaseString.ToCharArray());
        for (int charIndex = 0; charIndex < chars.Count; charIndex++)
        {
            var chr = chars[charIndex];
            if (charIndex > 0 && char.IsUpper(chr)) sb.Append(' ');
            sb.Append(chr);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Convert string to stream
    /// </summary>
    /// <param name="inString"></param>
    /// <returns></returns>
    public static Stream ConvertStringToStream(string inString)
    {
        byte[] byteArray = Encoding.UTF8.GetBytes(inString);
        var stream = new MemoryStream(byteArray);
        return stream;
    }

    /// <summary>
    /// Convert stream to string
    /// </summary>
    /// <param name="inStream"></param>
    /// <returns></returns>
    public static string ConvertStreamToString(Stream inStream)
    {
        var reader = new StreamReader(inStream);
        string text = reader.ReadToEnd();
        return text;
    }

    public static void StringToFile(string filePath, string text)
    {
        File.WriteAllText(filePath, text);
    }

    public static async Task<bool> StringToFileAsync(string filePath, string text)
    {
        var encodedText = Encoding.Unicode.GetBytes(text);
        using (var sourceStream = new FileStream(filePath,
            FileMode.Append, FileAccess.Write, FileShare.None,
            bufferSize: 4096, useAsync: true))
        {
            await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            return true;
        };
    }
    
    private static void StreamToFile(Stream inputStream, string outputFileFullPath)
    {
        using (var fileStream = File.Create(outputFileFullPath))
        {
            inputStream.Seek(0, SeekOrigin.Begin);
            inputStream.CopyTo(fileStream);
        }
    }
		
}
