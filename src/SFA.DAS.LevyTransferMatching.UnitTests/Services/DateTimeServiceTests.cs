using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Services;
using System;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Services;

[TestFixture]
public class DateTimeServiceTests
{
    private IDateTimeService dateTimeService;

    [Test]
    public void UtcNow_Returns_Provided_Date()
    {
        var expected = new DateTime(2021, 3, 22);
        dateTimeService = new DateTimeService(expected.ToShortDateString());

        var actual = dateTimeService.UtcNow;

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void UtcNow_Returns_Current_DateTime_When_None_Provided()
    {
        dateTimeService = new DateTimeService("");
        var expected = DateTime.UtcNow;

        var actual = dateTimeService.UtcNow;

        Assert.That(actual.Date, Is.EqualTo(expected.Date));
    }
}