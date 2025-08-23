
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Documents;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Documents;

public sealed class RequirementConfig : IEntityTypeConfiguration<Requirement>
{
    public void Configure(EntityTypeBuilder<Requirement> b)
    {
        b.ToTable("Requirements");
        b.HasKey(x => x.Id);
        b.Property(x => x.Status).HasMaxLength(32);
    }
}
