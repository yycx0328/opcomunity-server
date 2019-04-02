using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TimeHelper
{
    public static readonly DateTime InitUnixDateTime = new DateTime(1970, 1, 1);
    public static readonly DateTime InitNewDateTime = new DateTime(2000, 1, 1);

    /// <summary>
    /// Unix时间戳转为C#格式时间  
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTime ParseUnixDateTimeStamp(string timeStamp)
    {
        long seconds = TypeHelper.TryParse(timeStamp, (long)0);
        return InitUnixDateTime.AddSeconds((double)seconds);
    }

    /// <summary>
    /// 将Unix时间戳转换为DateTime类型时间
    /// </summary>
    /// <param name="l">long 型数字</param>
    /// <returns>DateTime</returns>
    public static System.DateTime ParseUnixDateTimeStamp(long l)
    {
        System.DateTime time = System.DateTime.MinValue;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(InitUnixDateTime);
        time = startTime.AddSeconds(l);
        return time;
    }

    /// <summary>
    /// 将Unix时间戳转换为DateTime类型时间
    /// </summary>
    /// <param name="d">double 型数字</param>
    /// <returns>DateTime</returns>
    public static System.DateTime ConvertIntDateTime(double d)
    {
        System.DateTime time = System.DateTime.MinValue;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(InitUnixDateTime);
        time = startTime.AddSeconds(d);
        return time;
    }
    /// <summary>
    /// 将c# DateTime时间格式转换为Unix时间戳格式
    /// </summary>
    /// <param name="time">时间</param>
    /// <returns>double</returns>
    public static double ConvertDateTimeInt(System.DateTime time)
    {
        double intResult = 0;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(InitUnixDateTime);
        intResult = (time - startTime).TotalSeconds;
        return intResult;
    }

    /// <summary>
    /// DateTime时间格式转换为Unix时间戳格式  
    /// 注：如果参数需要和php交互，请传入utc时间
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static long ConvertToUnixDateTimeStamp(System.DateTime time)
    {
        return Convert.ToInt64((time - InitUnixDateTime).TotalSeconds);
    }

    /// <summary>
    /// DateTime时间格式转换为Unix时间戳格式  
    /// 注：如果参数需要和php交互，请传入utc时间
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static long ConvertToUnixDateTimeStampMilliseconds(System.DateTime time)
    {
        return Convert.ToInt64((time - InitUnixDateTime).TotalMilliseconds);
    }
     
    /// <summary>
    /// DateTime时间格式转换为Unix时间戳格式  
    /// 注：如果参数需要和php交互，请传入utc时间
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static long ConvertBJTimeToUnixDateTimeStampMilliseconds(System.DateTime time)
    {
        return Convert.ToInt64((time - InitUnixDateTime.AddHours(8)).TotalMilliseconds);
    }

    /// <summary>
    /// 从2000年1月1日起的时间戳
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static long ConvertToNewDateTimeStamp(System.DateTime time)
    {
        return Convert.ToInt64((time - InitNewDateTime).TotalSeconds);
    }

    public static DateTime ParseMillsTimeStamp(string timeStamp)
    {
        long seconds = TypeHelper.TryParse(timeStamp, (long)0);
        return InitUnixDateTime.AddMilliseconds(seconds);
    }

    public static DateTime JsonToDateTime(string jsonDate)
    {
        string value = jsonDate.Substring(6, jsonDate.Length - 8);
        DateTimeKind kind = DateTimeKind.Utc;
        int index = value.IndexOf('+', 1);
        if (index == -1)
            index = value.IndexOf('-', 1);
        if (index != -1)
        {
            kind = DateTimeKind.Local;
            value = value.Substring(0, index);
        }
        long javaScriptTicks = long.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
        long InitialJavaScriptDateTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
        DateTime utcDateTime = new DateTime((javaScriptTicks * 10000) + InitialJavaScriptDateTicks, DateTimeKind.Utc);
        DateTime dateTime;
        switch (kind)
        {
            case DateTimeKind.Unspecified:
                dateTime = DateTime.SpecifyKind(utcDateTime.ToLocalTime(), DateTimeKind.Unspecified);
                break;
            case DateTimeKind.Local:
                dateTime = utcDateTime.ToLocalTime();
                break;
            default:
                dateTime = utcDateTime;
                break;
        }
        return dateTime;
    }

    public static DateTime ConvertToDateTime(string strTime, DateTime defaultTime)
    {
        DateTime time = defaultTime;
        try
        {
            time = Convert.ToDateTime(strTime);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
        }
        return time;
    }
     
    public static DateTime ConvertToDateTime(string strTime)
    {
        DateTime time = default(DateTime);
        try
        {
            time = Convert.ToDateTime(strTime);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
        }
        return time;
    }

    public static DateTime GetMonthFirstDay(DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }

    public static DateTime GetMonthLastDay(DateTime date)
    {
        var first = GetMonthFirstDay(date);
        return first.AddMonths(1).AddSeconds(-1);
    }

    /// <summary>
    /// 获取本周一的DateTime
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime GetMondayInWeek(DateTime dt)
    {
        int week = (int)dt.DayOfWeek;

        if (week == 0)
        {
            week = 7;
        }

        if (week == 1)
        {
            return dt;
        }
        else
        {
            DateTime result = dt.AddDays(-(week - 1));
            return result;
        }
    }

    /// <summary>
    /// 获取上周一的DateTime
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime GetLastMonday(DateTime dt)
    {
        return GetMondayInWeek(dt).Date.AddDays(-7);
    }

    /// <summary>
    /// 获取上周日的日期
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static DateTime GetLastSunday(DateTime dt)
    {
        return GetMondayInWeek(dt).Date.AddDays(-1);
    }

    /// <summary>
    /// 将时间转换成秒
    /// </summary>
    /// <param name="time">时间：hh:mm:ss</param>
    /// <returns>秒</returns>
    public static Int32 ConvertTime2Second(string time)
    {
        Int32 second = 0;
        string[] times = time.Split(':');
        second = Convert.ToInt32(times[0]) * 3600;
        second += Convert.ToInt32(times[1]) * 60;
        second += Convert.ToInt32(times[2]);
        return second;
    }

    /// <summary>
    /// 将年月日格式及时分秒格式字符组合成日期格式
    /// </summary>
    /// <param name="ymd">日期：yyyy-MM-dd</param>
    /// <param name="time">时间：hh:mm:ss</param>
    /// <returns>秒</returns>
    public static DateTime MergeTime(string ymd, string hms)
    {
        return Convert.ToDateTime(ymd + " " + hms);
    }
}
