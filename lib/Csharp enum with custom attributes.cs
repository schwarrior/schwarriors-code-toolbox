/// <remark>
/// Read custom attributes FormatName and HttpContentType
/// using extension method GetAttribute, defined below
/// </remark>
/// <code>
/// var frmt = ReportFormat.pdf;
/// var frntNm = frmt.GetAttribute<FormatNameAttribute>().Name;
/// </code>
public enum ReportFormat
{

    [FormatName("PDF")]
    [HttpContentType("application/pdf")]
    pdf,

    [FormatName("Excel")]
    [HttpContentType("application/vnd.ms-excel")]
    xls

}

public class FormatNameAttribute : Attribute
{
    public string Name { get; private set; }

    public FormatNameAttribute(string name)
    {
        this.Name = name;
    }
}

public class HttpContentTypeAttribute : Attribute
{
    public string Name { get; private set; }

    public HttpContentTypeAttribute(string name)
    {
        this.Name = name;
    }
}

public static class EnumExtensions
{
    public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
    {
        var enumType = value.GetType();
        var name = Enum.GetName(enumType, value);
        return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
    }
}
