using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProgram
{
    public class DelimitedParseUtility
    {
        public static List<List<string>> DelimitedFileToListOfStringLists(string csvFileName, char columnDelimiter = ',', char textDelimiter = '"')
        {
            const char substituteColumnDelimiter = '^';
            var parsedValues = new List<List<string>>();
            var counter = 0;
            string line;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(csvFileName);  
            while((line = file.ReadLine()) != null)  
            {
                var adjustedLine = new StringBuilder();
                var insideTextDelimiter = false;
                foreach (var c in line)
                {
                    //replace any columnDelimiters found inside of textDelimitted areas
                    var currentChar = c;
                    if (currentChar == textDelimiter)
                        insideTextDelimiter = !insideTextDelimiter;
                    if (insideTextDelimiter && currentChar == columnDelimiter)
                        currentChar = substituteColumnDelimiter;
                    adjustedLine.Append(currentChar);
                }
                var cells = new List<string>(adjustedLine.ToString().Split(columnDelimiter));
                var cellList = new List<string>();
                foreach (var c in cells)
                {
                    var cell = c;
                    if (cell.Length > 1 && cell.First() == textDelimiter && cell.Last() == textDelimiter)
                    {
                        //remove surrounding text delimit chars
                        cell = cell.Substring(1, cell.Length - 2);
                        //replace any substitute columnDelimiters with columnDelimiters
                        cell = cell.Replace(substituteColumnDelimiter, columnDelimiter);
                        //replace double text delimiters with a single
                        StringBuilder doubleTextDelimiter = new StringBuilder();
                        doubleTextDelimiter.Append(textDelimiter);
                        doubleTextDelimiter.Append(textDelimiter);
                        cell = cell.Replace(doubleTextDelimiter.ToString(), textDelimiter.ToString());
                    }
                    cellList.Add(cell);
                }
                parsedValues.Add(cellList);
                counter++;  
            }
            file.Close();  
            return parsedValues;
        }

        public static string CollectionToFlattenedString<T>(IEnumerable<T> collection, int characterDisplayLimit = int.MaxValue, int itemDisplayLimit = int.MaxValue, string noItemsString = "None")
        {
            var list = new List<T>(collection); //enable indexing. prevent multiple enums

            if (!list.Any()) return noItemsString;

            var sb = new StringBuilder();
            var listIndex = 0;
            for (listIndex = 0; listIndex < list.Count() && listIndex < itemDisplayLimit; listIndex++)
            {
                var itemString = list[listIndex].ToString();
                if (!string.IsNullOrEmpty(itemString))
                {
                    if (sb.Length > 0) { sb.Append(", "); }
                    sb.Append(itemString);
                }
            }

            //if there are more items than the itemDisplayLimit, show "and X more ..."
            var remaining = list.Count - (listIndex + 1);
            if (remaining > 0)
            {
                sb.AppendFormat(" and {0} more ...", remaining);
            }

            return StringToEllipsisLimitedString(sb.ToString(), characterDisplayLimit);
        }

        public static string StringToEllipsisLimitedString(string fullString, int characterLimit, string ellipsisText = "...")
        {
            if (fullString.Length > characterLimit)
                return fullString.Substring(0, characterLimit - ellipsisText.Length) + ellipsisText;
            return fullString;
        }

    }
}
