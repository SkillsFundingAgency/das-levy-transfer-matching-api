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

    public DateTime UtcNow => !string.IsNullOrWhiteSpace(_utcNowOverride) ? DateTime.Parse(_utcNowOverride) : DateTime.UtcNow;
}