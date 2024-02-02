using SFA.DAS.LevyTransferMatching.Extensions;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Extensions;

[TestFixture]
public class DateTimeExtensionsTests
{
    [TestCase("2022-04-01", "2021/22")]
    [TestCase("2022-04-05", "2021/22")]
    [TestCase("2022-04-06", "2022/23")]
    [TestCase("2023-01-01", "2022/23")]
    [TestCase("2023-04-05", "2022/23")]
    [TestCase("2023-04-06", "2023/24")]
    public void GetFinancialYear_Produces_Expected_Output(DateTime input, string expectedOutput)
    {
        var result = input.GetFinancialYear();
        Assert.That(result, Is.EqualTo(expectedOutput));
    }
}