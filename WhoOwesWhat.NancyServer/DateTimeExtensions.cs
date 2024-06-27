using System;
using System.Globalization;

namespace WhoOwesWhat.NancyServer
{
    public static class DateTimeExtensions
    {
        public static string ToNorwegianDate(this DateTime dateTime)
        {
            CultureInfo ci = new CultureInfo("nb-NO");

            const string format = "yyyy-MM-dd HH:mm:ss";

            // Converts the local DateTime to a string 
            // using the custom format string and display.
            return dateTime.ToString(format, ci);
        }
    }
}