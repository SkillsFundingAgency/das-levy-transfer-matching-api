using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.LevyTransferMatching.Data.Models;

namespace SFA.DAS.LevyTransferMatching.Data.Configurations;

public class EmployerAccountConfiguration : IEntityTypeConfiguration<EmployerAccount>
{
    public void Configure(EntityTypeBuilder<EmployerAccount> builder)
    {
        builder.ToTable("EmployerAccount");
        builder.HasKey(x => x.Id);
    }
}