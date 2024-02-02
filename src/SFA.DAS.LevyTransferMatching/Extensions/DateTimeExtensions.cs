namespace SFA.DAS.LevyTransferMatching.Extensions;

public static class DateTimeExtensions
{
    public static string GetFinancialYear(this DateTime dateValue)
    {
        if (dateValue.Month < 4 || (dateValue.Month == 4 && dateValue.Day < 6))
        {
            return $"{dateValue.Year - 1}/{dateValue:yy}";
        }

        return $"{dateValue.Year}/{dateValue.AddYears(1):yy}";
    }

    public static DateTime GetLastDayOfMonth(this DateTime dateValue)
    {
        var lastDayOfMonth = DateTime.DaysInMonth(dateValue.Year, dateValue.Month);
        return new DateTime(dateValue.Year, dateValue.Month, lastDayOfMonth);
    }
}