using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
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
            new ReferenceDataItem{ Id = "Option1", Description = "Option one", Hint = "Description for option 1"},
            new ReferenceDataItem{ Id = "Option2", Description = "Option two", Hint = "Description for option 2"},
            new ReferenceDataItem{ Id = "Option3", Description = "Option three", Hint = "Description for option 3"}
        };

        var compareLogic = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });
        var comparisonResult = compareLogic.Compare(expected, result);
        Assert.That(comparisonResult.AreEqual, Is.True);
    }


    public enum TestEnum
    {
        [ReferenceMetadata(Description = "Option one", Hint = "Description for option 1")]
        Option1 = 1,
        [ReferenceMetadata(Description = "Option two", Hint = "Description for option 2")]
        Option2 = 2,
        [ReferenceMetadata(Description = "Option three", Hint = "Description for option 3")]
        Option3 = 3
    }
}