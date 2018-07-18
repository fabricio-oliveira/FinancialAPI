using System;
namespace FinancialApi.Utils
{
    public static class DateUtil
    {
        private const string format = "dd-MM-yyyy";

        public static string DayFormat(this DateTime obj)
        {
            return obj.ToString(format);
        }

        public static bool IsSameDate(this DateTime obj, DateTime other)
        {
            return obj.ToString(format).Equals(other.ToString(format));
        }

    }
}
