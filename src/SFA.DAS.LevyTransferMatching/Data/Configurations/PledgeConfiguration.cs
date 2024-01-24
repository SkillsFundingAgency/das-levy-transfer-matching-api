using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data.Configurations;

public class PledgeConfiguration : IEntityTypeConfiguration<Pledge>
{
    public void Configure(EntityTypeBuilder<Pledge> builder)
    {
        builder.ToTable("Pledge");
        builder.HasKey(x => x.Id);
        
        builder.HasOne(x => x.EmployerAccount).WithMany();
        
        builder.Property(x => x.RowVersion).IsRowVersion();
        
        builder.HasMany(x => x.Locations).WithOne();
        builder.Metadata.FindNavigation(nameof(Pledge.Locations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        
        builder.HasMany(x => x.Applications).WithOne();
        builder.Metadata.FindNavigation(nameof(Pledge.Applications)).SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}