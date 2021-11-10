using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Data;

namespace SFA.DAS.LevyTransferMatching.UnitTests.DataFixture
{
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

        public LevyTransferMatchingDbContext DbContext { get; private set; }
    }
}
