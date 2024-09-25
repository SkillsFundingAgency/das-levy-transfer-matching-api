using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data.Configurations;

public class ApplicationLocationConfiguration : IEntityTypeConfiguration<ApplicationLocation>
{
    public void Configure(EntityTypeBuilder<ApplicationLocation> builder)
    {
        builder.ToTable("ApplicationLocation");
        builder.HasKey(x => x.Id);
    }
}