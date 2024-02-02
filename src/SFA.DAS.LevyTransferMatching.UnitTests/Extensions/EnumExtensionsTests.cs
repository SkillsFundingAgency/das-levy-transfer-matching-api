using KellermanSoftware.CompareNetObjects;
using SFA.DAS.LevyTransferMatching.Attributes;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Extensions;

[TestFixture]
public class EnumExtensionsTests
{
    [Test]
    public void ConvertToReferenceData_Produces_Expected_Output()
    {
        var result = EnumExtensions.ConvertToReferenceData<TestEnum>();

        var expected = new List<ReferenceDataItem>
        {
            new() { Id = "Option1", Description = "Option one", Hint = "Description for option 1"},
            new() { Id = "Option2", Description = "Option two", Hint = "Description for option 2"},
            new() { Id = "Option3", Description = "Option three", Hint = "Description for option 3"}
        };

        var compareLogic = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });
        var comparisonResult = compareLogic.Compare(expected, result);
        Assert.That(comparisonResult.AreEqual, Is.True);
    }


    private enum TestEnum
    {
        [ReferenceMetadata(Description = "Option one", Hint = "Description for option 1")]
        Option1 = 1,
        [ReferenceMetadata(Description = "Option two", Hint = "Description for option 2")]
        Option2 = 2,
        [ReferenceMetadata(Description = "Option three", Hint = "Description for option 3")]
        Option3 = 3
    }
}