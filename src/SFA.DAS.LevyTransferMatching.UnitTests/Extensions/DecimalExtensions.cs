using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Extensions;

[TestFixture]
public class DecimalExtensionsTests
{
    [Test]
    public void ToNearest_Accurately_Rounds_Up_OnMidPoint()
    {
        Assert.That(4_650m.ToNearest(100), Is.EqualTo(4700));
    }

    [Test]
    public void ToNearest_Accurately_Rounds_Down()
    {
        Assert.That(4_649m.ToNearest(100), Is.EqualTo(4600));
    }

    [Test]
    public void ToNearest_Accurately_Rounds_Up()
    {
        Assert.That(4_651m.ToNearest(100), Is.EqualTo(4700));
    }
}