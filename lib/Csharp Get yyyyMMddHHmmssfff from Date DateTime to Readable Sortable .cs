public class DateUtilities
{
    public static string GetYYYYMMDDMMSSMMMFromDateTime(DateTime inDateTime)
    {
        return string.Format("{0:yyyyMMddHHmmssfff}",inDateTime);
    }
}
