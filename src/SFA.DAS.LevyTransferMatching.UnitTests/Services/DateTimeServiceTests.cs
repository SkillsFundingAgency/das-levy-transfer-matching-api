using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Services;
using System;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Services
{
    [TestFixture]
    public class DateTimeServiceTests
    {
        private IDateTimeService dateTimeService;

        [Test]
        public void UtcNow_Returns_Provided_Date()
        {
            dateTimeService = new DateTimeService("22/03/2021 00:00:00");
            var expected = new DateTime(2021, 3, 22);

            var actual = dateTimeService.UtcNow;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UtcNow_Returns_Current_DateTime_When_None_Provided()
        {
            dateTimeService = new DateTimeService("");
            var expected = DateTime.UtcNow;

            var actual = dateTimeService.UtcNow;

            Assert.AreEqual(expected.Date, actual.Date);
        }
    }
}
