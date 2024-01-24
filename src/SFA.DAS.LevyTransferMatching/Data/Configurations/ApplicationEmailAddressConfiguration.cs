using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data.Configurations;

public class ApplicationEmailAddressConfiguration : IEntityTypeConfiguration<ApplicationEmailAddress>
{
    public void Configure(EntityTypeBuilder<ApplicationEmailAddress> builder)
    {
       builder.ToTable("ApplicationEmailAddress");
       builder.HasKey(x => x.Id);
    }
}