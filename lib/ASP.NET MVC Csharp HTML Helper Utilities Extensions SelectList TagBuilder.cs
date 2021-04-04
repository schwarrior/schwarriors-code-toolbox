 #region MVC Utils

public static SelectList ToSelectList<TEnum>(this TEnum enumObj, bool setDefaultItem = true)
    where TEnum : struct, IComparable, IFormattable, IConvertible
{
    var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                    select new { Id = e, Name = e.ToString() };
    SelectList returnedSelList;
    if (setDefaultItem)
    {
        returnedSelList = new SelectList(values, "Id", "Name", enumObj);
    }
    else
    {
        returnedSelList = new SelectList(values, "Id", "Name");
    }
    return returnedSelList;
}

public static TagBuilder TagTextToTagBuilder(MvcHtmlString mvcHtmlText)
{
    string inTagText = mvcHtmlText.ToString();
    TagBuilder outTagBuilder = TagTextToTagBuilder(inTagText);
    return outTagBuilder;
}

public static TagBuilder TagTextToTagBuilder(string tagText)
{
    var debugOutput = false;

    if (debugOutput)
    {
        Debug.WriteLine(string.Empty);
        Debug.WriteLine("TAG BUILDER IMPORT OUTPUT:");
        Debug.WriteLine(tagText);
    }

    //eliminate duplicate spaces
    string inTextSingleSpace = tagText.Replace("  ", " ");

    //split on >
    //isolate the first taglet
    string[] outerSplit = inTextSingleSpace.Split('>');
    string firstTaglet = outerSplit[0];

    //html encode everything inside of quotes
    Regex quotedTextRegex = new Regex("\"([^\"]*)\"");
    string firstTagletEncoded = quotedTextRegex.Replace(firstTaglet, new MatchEvaluator(m =>
        HttpContext.Current.Server.UrlEncode(m.ToString())
    ));

    //System.Diagnostics.Debug.WriteLine(firstTagletEncoded);

    //remove < and slashes
    firstTagletEncoded = firstTagletEncoded.Replace("\\", string.Empty);
    firstTagletEncoded = firstTagletEncoded.Replace("/", string.Empty);
    firstTagletEncoded = firstTagletEncoded.Replace("<", string.Empty);

    string[] outerAttribSplit = firstTagletEncoded.Split(' ');
    List<string> rawAttribs = new List<string>(outerAttribSplit);

    string tagName = rawAttribs[0];
    rawAttribs.RemoveAt(0); //take out the tagName

    TagBuilder tagBuilder = new TagBuilder(tagName);

    foreach (string rawAttrib in rawAttribs)
    {
        if (!string.IsNullOrEmpty(rawAttrib))
        {
            string[] attribParts = rawAttrib.Split('=');
            string attribName = attribParts[0];
            string attribValueRaw = attribParts[1];
            string attribValueDecoded = HttpContext.Current.Server.UrlDecode(attribValueRaw);
            //remove quotes
            attribValueDecoded = attribValueDecoded.Replace("\"", string.Empty);

            tagBuilder.Attributes.Add(attribName, attribValueDecoded);
        }
    }

    if (debugOutput) { Debug.WriteLine(tagBuilder.ToString()); }

    return tagBuilder;

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

public static MvcHtmlString AddHtmlAttributes(MvcHtmlString inMvcHtmlString, IDictionary<string, Object> htmlAttributes)
{
    TagBuilder tagBuilder = TagTextToTagBuilder(inMvcHtmlString);

    //todo find out if this handles class properly
    //tagBuilder.MergeAttributes(htmlAttributes);

    foreach (KeyValuePair<string, object> kv in htmlAttributes)
    {
        string attribName = kv.Key;

        //remove @
        attribName = attribName.Replace("@", string.Empty);
        //replace underscore with dash
        attribName = attribName.Replace("_", "-");

        string attribValue = string.Empty;
        if (kv.Value != null)
        {
            attribValue = kv.Value.ToString();
        }

        if (attribName.ToLower() == "class")
        {
            tagBuilder.AddCssClass(attribValue);
        }
        else
        {
            //add new attrib or replace existing
            tagBuilder.MergeAttribute(attribName, attribValue, true);
        }

    }

    MvcHtmlString outHtmlString = new MvcHtmlString(tagBuilder.ToString());
    return outHtmlString;

}

        #endregion