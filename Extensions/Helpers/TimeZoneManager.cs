﻿namespace TMODELOBASET_WebAPI_CS.Extensions.Helpers
{
    public class TimeZoneManager
    {
        private static readonly TimeZoneInfo TimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

        //public TimeZoneManager()
        //{
        //    //TimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        //    TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
        //}

        public static DateTime GetTime(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZone);
        }

        public static DateTime GetTimeNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZone);
        }
    }
}