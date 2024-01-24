using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data.Configurations;

public class PledgeLocationConfiguration : IEntityTypeConfiguration<PledgeLocation>
{
    public void Configure(EntityTypeBuilder<PledgeLocation> builder)
    {
        builder.ToTable("PledgeLocation");
    }
}