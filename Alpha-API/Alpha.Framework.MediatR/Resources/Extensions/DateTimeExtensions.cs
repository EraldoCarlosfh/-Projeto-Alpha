using TimeZoneConverter;

namespace Alpha.Framework.MediatR.Resources.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsSameAgeOrOlderThan(this DateTimeOffset date, int year)
        {
            try
            {
                var now = DateTimeOffset.Now.Date.AddYears(-1); //Removing 1 year so we can compare the dates 
                var age = new DateTime((now - date).Ticks);
                return age.Year >= year;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return false;
            }
        }

        public static bool IsSameAgeOrOlderThan(this DateTime date, int year)
        {
            DateTimeOffset dateOffset = date;
            return dateOffset.IsSameAgeOrOlderThan(year);
        }

        public static DateTime ToLocalTimeZone(this DateTime date)
        {
            if (date == null) return date;
            TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo("America/Sao_Paulo");
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(date, tzi);
            return localTime;
        }

        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;

            if (businessDays > fullWeekCount * 7)
            {
                int firstDayOfWeek = firstDay.DayOfWeek == DayOfWeek.Sunday
                    ? 7 : (int)firstDay.DayOfWeek;
                int lastDayOfWeek = lastDay.DayOfWeek == DayOfWeek.Sunday
                    ? 7 : (int)lastDay.DayOfWeek;

                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)
                    businessDays -= 1;
            }

            businessDays -= fullWeekCount + fullWeekCount;

            return businessDays;
        }

        public static DateTime AddBusinessDays(this DateTime date, int days)
        {
            if (days < 0)
            {
                throw new ArgumentException("days cannot be negative", "days");
            }

            if (days == 0) return date;

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
                days -= 1;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
                days -= 1;
            }

            date = date.AddDays(days / 5 * 7);
            int extraDays = days % 5;

            if ((int)date.DayOfWeek + extraDays > 5)
            {
                extraDays += 2;
            }

            return date.AddDays(extraDays);
        }

    }
}
