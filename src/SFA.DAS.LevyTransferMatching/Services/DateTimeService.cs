namespace SFA.DAS.LevyTransferMatching.Services;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
}

public class DateTimeService(string utcNowOverride) : IDateTimeService
{
    public DateTime UtcNow => !string.IsNullOrWhiteSpace(utcNowOverride) ? DateTime.Parse(utcNowOverride) : DateTime.UtcNow;
}