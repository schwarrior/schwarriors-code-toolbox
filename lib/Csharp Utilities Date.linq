<Query Kind="Program" />

void Main()
{
	var lastDayOfYear = new DateTime(DateTime.Now.Year,12,31);
	GetSecondsYearToDate().Dump();
	GetSecondsYearToDate(lastDayOfYear).Dump();
	GetMillisecondsYearToDate().Dump();
	GetMillisecondsYearToDate(lastDayOfYear).Dump();
	TicksToInt().Dump();
	TicksToInt(lastDayOfYear).Dump();
	TicksToInt(lastDayOfYear.Ticks).Dump();
}

public static int GetSecondsYearToDate()
{
	return GetSecondsYearToDate(DateTime.Now);
}

public static int GetSecondsYearToDate(DateTime dateTime)
{
	var newYear = new DateTime(dateTime.Year,1,1);
	var fromNewYearTimeSpan = dateTime - newYear;
	return (int)Math.Floor(fromNewYearTimeSpan.TotalSeconds);
}

public static long GetMillisecondsYearToDate()
{
	return GetMillisecondsYearToDate(DateTime.Now);
}

public static long GetMillisecondsYearToDate(DateTime dateTime)
{
	var newYear = new DateTime(dateTime.Year,1,1);
	var fromNewYearTimeSpan = dateTime - newYear;
	return (long)Math.Floor(fromNewYearTimeSpan.TotalMilliseconds);
}

public static int TicksToInt()
{
	return TicksToInt(DateTime.Now);
}


public static int TicksToInt(DateTime dateTime)
{
	return TicksToInt(dateTime.Ticks);
}

public static int TicksToInt(long ticks)
{
	return (int)(ticks % int.MaxValue);
}

/// <summary>
/// Get yyyyMMddHHmmssfff from Date DateTime to Readable Sortable
/// </summary>
/// <param name="inDateTime"></param>
/// <returns></returns>
public static string GetYYYYMMDDMMSSMMMFromDateTime(DateTime inDateTime)
{
    return string.Format("{0:yyyyMMddHHmmssfff}",inDateTime);
}

public static bool TryParseYYYYMMDDToDateTime(string dateTimeString, out DateTime parsedDateTime)
{
    int temp;
    parsedDateTime = default(DateTime);
    if (!string.IsNullOrEmpty(dateTimeString) && dateTimeString.Length >= 8 && int.TryParse(dateTimeString.Substring(0, 8), out temp))
    {
        int year = int.Parse(dateTimeString.Substring(0, 4));
        int month = int.Parse(dateTimeString.Substring(4, 2));
        if (month > 12) return false;
        int day = int.Parse(dateTimeString.Substring(6, 2));
        if (day > 31) return false;
        parsedDateTime = new DateTime(year, month, day);
        //try to parse the time
        if (dateTimeString.Length >= 14 && int.TryParse(dateTimeString.Substring(8, 6), out temp))
        {
            int hour = int.Parse(dateTimeString.Substring(8, 2));
            if (hour > 24) return true;
            int min = int.Parse(dateTimeString.Substring(10, 2));
            if (min > 60) return true;
            int sec = int.Parse(dateTimeString.Substring(12, 2));
            if (sec > 60) return true;
            parsedDateTime = new DateTime(year, month, day, hour, min, sec);
        }
        return true;
    }
    return false;
}