public class ConsoleUtilities
    {
    public static T GetUserSelection<T>(string prompt, IEnumerable<T> selectItems)
    {
        //bug: using this causes the next Console.ReadLine() to fire automatically. It leaves /r/n in the buffer
        if (!selectItems.Any()) throw new ArgumentOutOfRangeException("Select Item list cannot be null or empty");
        Console.WriteLine(prompt);
        var selectList = selectItems.ToList();
        for (var itemIndex = 0; itemIndex < selectList.Count; itemIndex++)
        {
            Console.WriteLine("{0}. {1}", NumberToSingleCharIdentifier(itemIndex + 1), selectList[itemIndex]);
        }
        int userIndexSelection = 0;
        while (userIndexSelection < 1 || userIndexSelection > selectList.Count)
        {
            var userInput = Console.ReadKey();
            var userInputChar = userInput.KeyChar;
            try
            {
                userIndexSelection = SingleCharIndentifierToNumber(userInputChar);
            }
            catch { }
        }
        Console.WriteLine();
        return selectList[userIndexSelection - 1];
    }

    public static char NumberToSingleCharIdentifier(int number)
    {
        const int maxNumber = 9 + 26;
        if (number < 0 || number > maxNumber) throw new ArgumentOutOfRangeException("Numbers to be converted to a single character identifier must be between 0 and " + maxNumber);
        if (number <= 9) return number.ToString()[0];
        const int charOffset = (int)'A';
        return (char)(number + charOffset);
    }

    public static int SingleCharIndentifierToNumber(char identifier)
    {
        if (Char.IsDigit(identifier)) return Int32.Parse(identifier.ToString());
        if (!Char.IsLetter(identifier)) throw new ArgumentOutOfRangeException("Only digit and letter character identifiers can be converted to numbers");
        if (Char.IsLower(identifier)) identifier = identifier.ToString().ToUpper()[0];
        const int charOffset = (int)'A';
        return (int)identifier - charOffset;
    }

    ///// <summary>
    ///// Example usage
    ///// </summary>
    ///// <param name="args"></param>
    //static void Main(string[] args)
    //{
    //    Console.WriteLine("File Renaming Random File Utility");

    //    const string mockMode = "Mock Mode";
    //    const string liveMode = "Live Mode";
    //    var execModes = new List<string>(new string[] { mockMode, liveMode });
    //    var selectedMode = ConsoleUtilities.GetUserSelection("Select", execModes);
    //    FileManager mgr = selectedMode == mockMode ? new FileManager(new MockFileUpdateManager()) : new FileManager(new LiveFileUpdateManager());

    //    const string createFile = "Create File";
    //    const string partialClear = "Partial Clear";
    //    const string fullClear = "Full Clear";
    //    var functions = new List<string>(new string[] { createFile, partialClear, fullClear });
    //    var selectedFunction = ConsoleUtilities.GetUserSelection("Select", functions);
    //    if (selectedFunction == createFile) mgr.CreateFile();
    //    if (selectedFunction == partialClear) mgr.ClearFilePartial();
    //    if (selectedFunction == fullClear) mgr.ClearFileFull();

    //    Console.WriteLine();
    //    Console.WriteLine("Press Enter to exit");
    //    Console.ReadLine();
    //}

}
