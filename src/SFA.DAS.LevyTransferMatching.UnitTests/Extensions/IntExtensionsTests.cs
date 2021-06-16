using AutoFixture;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Enums;
using System.Linq;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Extensions
{
    [TestFixture]
    public class IntExtensionsTests
    {
        private Fixture _fixture;

        public IntExtensionsTests()
        {
            _fixture = new Fixture();
        }

        [Test]
        public void Summed_Sector_Flag_Values_Convert_Back_Correctly()
        {
            var expectedSectors = _fixture.CreateMany<Sector>(5);
            var summed = expectedSectors.Cast<int>().Sum();

            var actualSectors = summed.GetFlags<Sector>();

            CollectionAssert.AreEqual(expectedSectors, actualSectors);
        }
    }
}