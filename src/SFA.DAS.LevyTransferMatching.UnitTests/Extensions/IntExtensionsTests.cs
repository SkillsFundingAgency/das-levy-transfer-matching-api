﻿using SFA.DAS.LevyTransferMatching.Extensions;
using SFA.DAS.LevyTransferMatching.Models.Enums;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Extensions;

[TestFixture]
public class IntExtensionsTests
{
    private readonly Fixture _fixture = new();

    [Test]
    public void Summed_Sector_Flag_Values_Convert_Back_Correctly()
    {
        var expectedSectors = _fixture.CreateMany<Sector>(5).ToArray();
        var summed = expectedSectors.Cast<int>().Sum();

        var actualSectors = summed.GetFlags<Sector>();

        actualSectors.Should().BeEquivalentTo(expectedSectors);
    }
}