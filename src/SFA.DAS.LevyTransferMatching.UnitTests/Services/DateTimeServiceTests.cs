using SFA.DAS.LevyTransferMatching.Services;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Services;

[TestFixture]
public class DateTimeServiceTests
{
    private IDateTimeService _dateTimeService;

    [Test]
    public void UtcNow_Returns_Provided_Date()
    {
        var expected = new DateTime(2021, 3, 22);
        _dateTimeService = new DateTimeService(expected.ToShortDateString());

        var actual = _dateTimeService.UtcNow;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void UtcNow_Returns_Current_DateTime_When_None_Provided()
    {
        _dateTimeService = new DateTimeService("");
        var expected = DateTime.UtcNow;

        var actual = _dateTimeService.UtcNow;

        Assert.That(actual.Date, Is.EqualTo(expected.Date));
    }
}