using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Extensions
{
    [TestFixture]
    public class DecimalExtensionsTests
    {
        [Test]
        public void ToNearest_Accurately_Rounds_Up_OnMidPoint()
        {
            Assert.AreEqual(4700, 4_650m.ToNearest(100));
        }

        [Test]
        public void ToNearest_Accurately_Rounds_Down()
        {
            Assert.AreEqual(4600, 4_649m.ToNearest(100));
        }

        [Test]
        public void ToNearest_Accurately_Rounds_Up()
        {
            Assert.AreEqual(4700, 4_651m.ToNearest(100));
        }
    }
}
