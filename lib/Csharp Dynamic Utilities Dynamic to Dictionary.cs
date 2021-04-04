public class CollectionUtilities
{
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
