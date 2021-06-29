using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Tags;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Extensions
{
    [TestFixture]
    public class EnumExtensionsTests
    {
        [Test]
        public void ConvertToTags_Produces_Expected_Output()
        {
            var result = EnumExtensions.ConvertToTags<TestEnum>();

            var expected = new List<Tag>
            {
                new Tag{ TagId = "Option1", Description = "Option one", ExtendedDescription = "Description for option 1"},
                new Tag{ TagId = "Option2", Description = "Option two", ExtendedDescription = "Description for option 2"},
                new Tag{ TagId = "Option3", Description = "Option three", ExtendedDescription = "Description for option 3"}
            };

            var compareLogic = new CompareLogic(new ComparisonConfig { IgnoreObjectTypes = true });
            var comparisonResult = compareLogic.Compare(expected, result);
            Assert.IsTrue(comparisonResult.AreEqual);
        }


        public enum TestEnum
        {
            [Display(Name = "Option one", Description = "Description for option 1")]
            Option1 = 1,
            [Display(Name = "Option two", Description = "Description for option 2")]
            Option2 = 2,
            [Display(Name = "Option three", Description = "Description for option 3")]
            Option3 = 3
        }
    }
}
