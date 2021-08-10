using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Data;

namespace SFA.DAS.LevyTransferMatching.UnitTests.DataFixture
{
    public class LevyTransferMatchingDbContextFixture : IDisposable
    {
        public LevyTransferMatchingDbContext DbContext { get; set; }

        public LevyTransferMatchingDbContextFixture()
        {
            ResetDbContext();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }

        protected void ResetDbContext()
        {
            var options = new DbContextOptionsBuilder<LevyTransferMatchingDbContext>()
                .UseInMemoryDatabase($"SFA.DAS.LevyTransferMatching.Database_{DateTime.UtcNow.ToFileTimeUtc()}")
                .Options;

            DbContext = new LevyTransferMatchingDbContext(options);
        }
    }
}
