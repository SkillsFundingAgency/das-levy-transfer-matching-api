using System;

namespace SFA.DAS.LevyTransferMatching.Services;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
}

public class DateTimeService : IDateTimeService
{
    private readonly string _utcNowOverride;

    public DateTimeService(string utcNowOverride)
    {
        _utcNowOverride = utcNowOverride;
    }

    public DateTime UtcNow
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_utcNowOverride))
            {
                return DateTime.Parse(_utcNowOverride);
            }
            else
            {
                return DateTime.UtcNow;
            }
        }
    }
}