using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SFA.DAS.LevyTransferMatching.Data.Configurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<Models.Application>
{
    public void Configure(EntityTypeBuilder<Models.Application> builder)
    {
        builder.ToTable("Application");
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.EmployerAccount).WithMany();
        builder.HasOne(x => x.Pledge).WithMany(x => x.Applications);
        builder.Property(x => x.RowVersion).IsRowVersion();
        builder.HasMany(x => x.EmailAddresses).WithOne();
        builder.Metadata.FindNavigation(nameof(Models.Application.EmailAddresses)).SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.HasMany(x => x.StatusHistory).WithOne();
        builder.Metadata.FindNavigation(nameof(Models.Application.StatusHistory)).SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.HasMany(x => x.ApplicationLocations).WithOne();
        builder.Metadata.FindNavigation(nameof(Models.Application.ApplicationLocations)).SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.HasMany(x => x.ApplicationCostProjections).WithOne().IsRequired(true);
        builder.Metadata.FindNavigation(nameof(Models.Application.ApplicationCostProjections)).SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}