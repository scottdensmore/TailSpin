




 




namespace TailSpin.Web.Survey.Shared
{
    using System;
    using System.Globalization;

    public static class DateTimeExtensions
    {
        public static string GetFormatedTicks(this DateTime dateTime)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:D19}", dateTime.Ticks);
        }
    }
}
