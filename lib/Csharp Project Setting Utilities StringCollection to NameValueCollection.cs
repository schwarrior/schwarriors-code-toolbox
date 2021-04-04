using System.Collections.Specialized;

public class ProjectSettingUtilities
{
    /// <summary>
    /// VS .NET Project Settings dont allow any key / value type collections
    /// But it does allow StringCollections
    /// Can store a pipe delimitted StringCollection in project settings like
    ///     MyNameA|MyValueA
    ///     MyNameB|MyValueB
    /// and use this util method to convert it to a NameValueCollection
    /// </summary>
    /// <param name="stringCollection"></param>
    /// <param name="delimiter"></param>
    /// <returns></returns>
    public static NameValueCollection ParseDelimittedStringCollection(StringCollection stringCollection, char delimiter = '|')
    {
        var nvCollection = new NameValueCollection();
        foreach (var s in stringCollection)
        {
            var sSplit = s.Split(delimiter);
            var name = string.Empty;
            if (sSplit.Length > 0) name = sSplit[0];
            var value = string.Empty;
            if (sSplit.Length > 1) value = sSplit[1];
            nvCollection.Add(name, value);
        }
        return nvCollection;
    }
}
