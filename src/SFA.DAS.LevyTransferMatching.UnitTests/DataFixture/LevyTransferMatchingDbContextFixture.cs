using System;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LevyTransferMatching.Data;

namespace SFA.DAS.LevyTransferMatching.UnitTests.DataFixture
{
    public class LevyTransferMatchingDbContextFixture : IDisposable
    {
        public LevyTransferMatchingDbContext DbContext { get; }

        public LevyTransferMatchingDbContextFixture()
        {
            var options = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>()
                .UseInMemoryDatabase($"SFA.DAS.LevyTransferMatching.Database_{DateTime.UtcNow.ToFileTimeUtc()}")
                .Options;

            DbContext = new LevyTransferMatchingDbContext(options);
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
