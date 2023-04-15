using System;

namespace Net.Essentials
{
    public static class DateExtensions
    {
        static readonly DateTime BaseDate = new DateTime(year: 2019, month: 3, day: 11);

        public static int ToWeekNumber(this DateTime date)
        {
            return (int)((date - BaseDate).TotalDays / 7);
        }

        public static DateTime GetFirstDayOfWeek(this int weeknumber)
        {
            var days = weeknumber * 7;
            return BaseDate.AddDays(days);
        }

        public static DateTime GetLastDayOfWeek(this int weeknumber)
        {
            return (weeknumber + 1).GetFirstDayOfWeek();
        }

        public static DateTimeOffset GetTodayStartUniversalTime(TimeZoneInfo tz)
        {
            return GetDayStartUniversalTime(tz, DateTimeOffset.UtcNow);
        }

        public static DateTimeOffset GetDayStartUniversalTime(TimeZoneInfo tz, DateTimeOffset utc)
        {
            return GetDayStartUniversalTime(tz, GetLocalTime(tz, utc));
        }

        public static DateTime GetLocalTime(TimeZoneInfo tz, DateTimeOffset utc)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utc.UtcDateTime, tz);
        }

        public static DateTimeOffset GetUniversalTime(TimeZoneInfo tz, DateTime local)
        {
            return TimeZoneInfo.ConvertTimeToUtc(new DateTime(
                year: local.Year, month: local.Month, day: local.Day,
                hour: local.Hour, minute: local.Minute, second: local.Second), tz);
        }

        public static DateTimeOffset GetDayStartUniversalTime(TimeZoneInfo tz, DateTime dt)
        {
            var offset = tz.GetUtcOffset(dt);
            return new DateTimeOffset(year: dt.Year, month: dt.Month, day: dt.Day, hour: 0, minute: 0, second: 0, millisecond: 0, offset);
        }

        public static DateTime GetDateInTimeZone(TimeZoneInfo tz, DateTime dt)
        {
            var dto = GetDayStartUniversalTime(tz, dt);
            return dto.Date;
        }

        public static TimeSpan SnapToNearest5Minute(this TimeSpan value)
        {
            return SnapToNearestNMinute(value, 5);
        }

        public static TimeSpan SnapToNearestNMinute(this TimeSpan value, int step)
        {
            value = value.WithoutSeconds();
            var minute = step * (int)(0.5 + value.Minutes / (double)step);
            return minute > 60
                ? value.Add(TimeSpan.FromMinutes(-value.Minutes)).Add(TimeSpan.FromHours(1))
                : value.Add(TimeSpan.FromMinutes(-value.Minutes)).Add(TimeSpan.FromMinutes(minute));
        }

        public static TimeSpan SnapToNearestNMinuteCeil(this TimeSpan value, int step)
        {
            value = value.WithoutSeconds();
            var minute = (int)step * (int)Math.Ceiling((double)value.Minutes / (double)step);
            return minute > 60
                ? value.Add(TimeSpan.FromMinutes(-value.Minutes)).Add(TimeSpan.FromHours(1))
                : value.Add(TimeSpan.FromMinutes(-value.Minutes)).Add(TimeSpan.FromMinutes(minute));
        }

        public static TimeSpan SnapToNearestNMinuteFloor(this TimeSpan value, int step)
        {
            value = value.WithoutSeconds();
            var minute = (int)step * (int)((double)value.Minutes / (double)step);
            return minute > 60
                ? value.Add(TimeSpan.FromMinutes(-value.Minutes)).Add(TimeSpan.FromHours(1))
                : value.Add(TimeSpan.FromMinutes(-value.Minutes)).Add(TimeSpan.FromMinutes(minute));
        }

        public static DateTimeOffset WithoutSeconds(this DateTimeOffset dt)
        {
            return dt - TimeSpan.FromSeconds(dt.TimeOfDay.Seconds) - TimeSpan.FromMilliseconds(dt.TimeOfDay.Milliseconds);
        }

        public static DateTime WithoutSeconds(this DateTime dt)
        {
            return dt - TimeSpan.FromSeconds(dt.TimeOfDay.Seconds) - TimeSpan.FromMilliseconds(dt.TimeOfDay.Milliseconds);
        }

        public static TimeSpan WithoutSeconds(this TimeSpan ts)
        {
            return new TimeSpan((int)ts.TotalHours, ts.Minutes, 0);
        }

        public static string ToHourMinuteString(this TimeSpan ts)
        {
            var totalHours = (int)ts.TotalHours;
            if (ts < TimeSpan.Zero)
            {
                ts = ts.Duration();
            }
            return totalHours + ts.ToString("'h'mm");
        }

        public static DateTimeOffset GetPreviousDay(TimeZoneInfo tz, DateTimeOffset dt)
        {
            var local = GetLocalTime(tz, dt);
            var yesterday = local.AddDays(-1);
            var utc = GetUniversalTime(tz, yesterday);
            return utc;
        }

        public static DateTimeOffset GetNextDay(TimeZoneInfo tz, DateTimeOffset dt)
        {
            var local = GetLocalTime(tz, dt);
            var yesterday = local.AddDays(1);
            var utc = GetUniversalTime(tz, yesterday);
            return utc;
        }
    }
}