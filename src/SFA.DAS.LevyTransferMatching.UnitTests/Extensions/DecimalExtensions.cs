using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Extensions;

[TestFixture]
public class DecimalExtensionsTests
{
    [Test]
    public void ToNearest_Accurately_Rounds_Up_OnMidPoint()
    {
        4_650m.ToNearest(100).Should().Be(4700);
    }

    [Test]
    public void ToNearest_Accurately_Rounds_Down()
    {
        4_649m.ToNearest(100).Should().Be(4600);
    }

    [Test]
    public void ToNearest_Accurately_Rounds_Up()
    {
        4_651m.ToNearest(100).Should().Be(4700);
    }
}