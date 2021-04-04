using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace ToolBox.Common.Utilities
{

    public static class Conversion
    {
        public static List<int> CsvStringToIntList(string csvString)
        {
            var intList = new List<int>();
            var cleanCsvString = csvString.Replace(" ", string.Empty);
            if (string.IsNullOrEmpty(cleanCsvString)) return intList;
            var splitStringIDs = cleanCsvString.Split(',');
            foreach (string stringId in splitStringIDs)
            {
                int foundID = -1;
                if (int.TryParse(stringId, out foundID))
                {
                    intList.Add(int.Parse(stringId));
                }
            }
            return intList;
        }

        public static List<int> ToIntList(this string csvString)
        {
            var intList = new List<int>();
            var cleanCsvString = csvString.Replace(" ", string.Empty);
            if (string.IsNullOrEmpty(cleanCsvString)) return intList;
            var splitStringIDs = cleanCsvString.Split(',');
            foreach (string stringId in splitStringIDs)
            {
                int foundID = -1;
                if (int.TryParse(stringId, out foundID))
                {
                    intList.Add(int.Parse(stringId));
                }
            }
            return intList;
        }

        public static string IntListToCSVString(List<int> intList)
        {
            System.Text.StringBuilder csv = new System.Text.StringBuilder();

            foreach (int i in intList)
            {
                if (csv.Length > 0) { csv.Append(","); }
                csv.Append(i);
            }

            return csv.ToString();

        }

        public static List<Guid> CSVStringToGuidList(string csvString)
        {
            List<Guid> guidList = new List<Guid>();

            string cleanCsvString = csvString.Replace(" ", string.Empty);

            if (!string.IsNullOrEmpty(cleanCsvString))
            {
                string[] splitStringIDs = cleanCsvString.Split(',');
                foreach (string stringId in splitStringIDs)
                {
                    Guid foundGuid;
                    if (Guid.TryParse(stringId, out foundGuid))
                    {
                        guidList.Add(foundGuid);
                    }
                }
            }

            return guidList;
        }

        public static string GuidListToCSVString(List<Guid> guidList)
        {
            System.Text.StringBuilder csv = new System.Text.StringBuilder();

            foreach (Guid g in guidList)
            {
                if (csv.Length > 0) { csv.Append(","); }
                csv.Append(g.ToString());
            }

            return csv.ToString();

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

        public static Dictionary<string, string> EnumToDictionary<T>()
        {
            return EnumToDictionary<T>(string.Empty, string.Empty);
        }

        public static Dictionary<string, string> EnumToDictionary<T>(string headerValue, string headerName)
        {
            Dictionary<string, string> resultDict = new Dictionary<string, string>();

            //List<string> EnumNames = new List<string>(Enum.GetNames(typeof(T)));

            //List<string> EnumValues = new List<string>(Enum.GetValues(typeof(T)).Cast<string>());

            Array EnumValues = Enum.GetValues(typeof(T));

            if (!string.IsNullOrEmpty(headerValue) && !string.IsNullOrEmpty(headerName))
            {
                resultDict.Add(headerValue, headerName);
            }

            foreach (object objVal in EnumValues)
            {
                T val = (T)objVal;
                int valInt = Convert.ToInt32(val);
                resultDict.Add((valInt).ToString(), val.ToString());
            }

            //for(int idx = 0; idx < EnumValues.Count; idx++)
            //{
            //    resultDict.Add(EnumValues[idx], EnumNames[idx]);
            //}

            return resultDict;

        }



        public static SelectList DictionaryToSelectList(Dictionary<string, string> inDict)
        {
            SelectList sl = new SelectList(inDict, "Key", "Value", inDict.First());
            return sl;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo propInfo in oProps)
                {
                    dr[propInfo.Name] = propInfo.GetValue(rec, null) == null
                        ? DBNull.Value
                        : propInfo.GetValue
                            (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static IDictionary<string, object> DynamicToDictionary(dynamic dynamicObject)
        {

            Dictionary<string, object> returnDict = new Dictionary<string, object>();

            PropertyInfo[] propertyInfos = dynamicObject.GetType().GetProperties();

            // exampine added properties
            foreach (PropertyInfo propInfo in propertyInfos)
            {
                string memberName = propInfo.Name;

                object memberValue = dynamicObject.GetType().GetProperty(memberName).GetValue(dynamicObject);

                returnDict.Add(memberName, memberValue);

            }

            return returnDict;
        }






    }
}
