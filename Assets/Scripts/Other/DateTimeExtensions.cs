using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class DateTimeExtensions
{

    private static readonly DateTime UnixEpochDateTimeUtc = new DateTime(621355968000000000L, DateTimeKind.Utc);
    private static readonly DateTime UnixEpochDateTimeUnspecified = new DateTime(621355968000000000L, DateTimeKind.Unspecified);
    private static readonly DateTime MinDateTimeUtc = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public const long UnixEpoch = 621355968000000000;

    public static DateTime FromUnixTime(this int unixTime)
    {
        return UnixEpochDateTimeUtc + TimeSpan.FromSeconds(unixTime);
    }

    public static DateTime FromUnixTime(this double unixTime)
    {
        return UnixEpochDateTimeUtc + TimeSpan.FromSeconds(unixTime);
    }

    public static DateTime FromUnixTime(this long unixTime)
    {
        return UnixEpochDateTimeUtc + TimeSpan.FromSeconds(unixTime);
    }

    public static long ToUnixTimeMsAlt(this DateTime dateTime)
    {
        return (dateTime.ToStableUniversalTime().Ticks - 621355968000000000L) / 10000L;
    }

    public static long ToUnixTimeMs(this DateTime dateTime)
    {
        return (long)dateTime.ToDateTimeSinceUnixEpoch().TotalMilliseconds;
    }

    public static long ToUnixTime(this DateTime dateTime)
    {
        return dateTime.ToDateTimeSinceUnixEpoch().Ticks / 10000000L;
    }

    private static TimeSpan ToDateTimeSinceUnixEpoch(this DateTime dateTime)
    {
        var dateTimeUtc = dateTime;
        if (dateTime.Kind != DateTimeKind.Utc)
        {
            dateTimeUtc = dateTime.Kind != DateTimeKind.Unspecified || !(dateTime > DateTime.MinValue) || !(dateTime < DateTime.MaxValue) ? dateTime.ToStableUniversalTime() : DateTime.SpecifyKind(dateTime.Subtract(GetLocalTimeZoneInfo().GetUtcOffset(dateTime)), DateTimeKind.Utc);
        }
        return dateTimeUtc.Subtract(UnixEpochDateTimeUtc);
    }

    public static TimeZoneInfo GetLocalTimeZoneInfo()
    {
        try
        {
            return TimeZoneInfo.Local;
        }
        catch (Exception)
        {
            return TimeZoneInfo.Utc;
        }
    }

    public static long ToUnixTimeMs(this long ticks)
    {
        return (ticks - 621355968000000000L) / 10000L;
    }

    public static DateTime FromUnixTimeMs(this double msSince1970)
    {
        return UnixEpochDateTimeUtc + TimeSpan.FromMilliseconds(msSince1970);
    }

    public static DateTime FromUnixTimeMs(this long msSince1970)
    {
        return UnixEpochDateTimeUtc + TimeSpan.FromMilliseconds(msSince1970);
    }

    public static DateTime FromUnixTimeMs(this long msSince1970, TimeSpan offset)
    {
        return DateTime.SpecifyKind(UnixEpochDateTimeUnspecified + TimeSpan.FromMilliseconds(msSince1970) + offset, DateTimeKind.Local);
    }

    public static DateTime FromUnixTimeMs(this double msSince1970, TimeSpan offset)
    {
        return DateTime.SpecifyKind(UnixEpochDateTimeUnspecified + TimeSpan.FromMilliseconds(msSince1970) + offset, DateTimeKind.Local);
    }

    public static DateTime FromUnixTimeMs(string msSince1970)
    {
        long result;
        if (long.TryParse(msSince1970, out result))
        {
            return result.FromUnixTimeMs();
        }
        return double.Parse(msSince1970).FromUnixTimeMs();
    }

    public static DateTime FromUnixTimeMs(string msSince1970, TimeSpan offset)
    {
        long result;
        if (long.TryParse(msSince1970, out result))
        {
            return result.FromUnixTimeMs(offset);
        }
        return double.Parse(msSince1970).FromUnixTimeMs(offset);
    }

    public static DateTime RoundToMs(this DateTime dateTime)
    {
        return new DateTime(dateTime.Ticks / 10000L * 10000L, dateTime.Kind);
    }

    public static DateTime RoundToSecond(this DateTime dateTime)
    {
        return new DateTime(dateTime.Ticks / 10000000L * 10000000L, dateTime.Kind);
    }

    public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
    {
        return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
    }

    public static bool IsEqualToTheSecond(this DateTime dateTime, DateTime otherDateTime)
    {
        return dateTime.ToStableUniversalTime().RoundToSecond().Equals(otherDateTime.ToStableUniversalTime().RoundToSecond());
    }

    public static string ToTimeOffsetString(this TimeSpan offset, string seperator = "")
    {
        var hours = Math.Abs(offset.Hours).ToString(CultureInfo.InvariantCulture);
        var minutes = Math.Abs(offset.Minutes).ToString(CultureInfo.InvariantCulture);
        return (offset < TimeSpan.Zero ? "-" : "+") + (hours.Length == 1 ? "0" + hours : hours) + seperator + (minutes.Length == 1 ? "0" + minutes : minutes);
    }

    public static TimeSpan FromTimeOffsetString(this string offsetString)
    {
        if (!offsetString.Contains(":"))
        {
            offsetString = offsetString.Insert(offsetString.Length - 2, ":");
        }
        offsetString = offsetString.TrimStart('+');
        return TimeSpan.Parse(offsetString);
    }

    public static DateTime ToStableUniversalTime(this DateTime dateTime)
    {
        if (dateTime.Kind == DateTimeKind.Utc)
        {
            return dateTime;
        }
        if (dateTime == DateTime.MinValue)
        {
            return MinDateTimeUtc;
        }
        return dateTime.ToUniversalTime();
    }

    public static string FmtSortableDate(this DateTime from)
    {
        return from.ToString("yyyy-MM-dd");
    }

    public static string FmtSortableDateTime(this DateTime from)
    {
        return from.ToString("u");
    }

    public static DateTime LastMonday(this DateTime from)
    {
        return from.Date.AddDays(-(int)from.DayOfWeek + 1);
    }

    public static DateTime StartOfLastMonth(this DateTime from)
    {
        var dateTime = from.Date;
        var year = dateTime.Year;
        dateTime = from.Date;
        var month = dateTime.Month;
        var day = 1;
        dateTime = new DateTime(year, month, day);
        return dateTime.AddMonths(-1);
    }

    public static DateTime EndOfLastMonth(this DateTime from)
    {
        var dateTime = from.Date;
        var year = dateTime.Year;
        dateTime = from.Date;
        var month = dateTime.Month;
        var day = 1;
        dateTime = new DateTime(year, month, day);
        return dateTime.AddDays(-1.0);
    }
}
