﻿using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;

namespace SFA.DAS.LevyTransferMatching.UnitTests.DataFixture;

public class LevyTransferMatchingDbContextFixture
{
    [SetUp]
    public void BaseSetup()
    {
        var options = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>()
            .UseInMemoryDatabase($"SFA.DAS.LevyTransferMatching.Database_{DateTime.UtcNow.ToFileTimeUtc()}")
            .Options;

        DbContext = new LevyTransferMatchingDbContext(options);
    }

    [TearDown]
    public void TearDown() => DbContext?.Dispose();

    protected LevyTransferMatchingDbContext DbContext { get; private set; }
}