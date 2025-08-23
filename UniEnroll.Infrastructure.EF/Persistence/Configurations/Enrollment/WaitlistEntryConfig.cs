
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Enrollment;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Enrollment;

public sealed class WaitlistEntryConfig : IEntityTypeConfiguration<WaitlistEntry>
{
    public void Configure(EntityTypeBuilder<WaitlistEntry> b)
    {
        b.ToTable("Waitlist");
        b.HasKey(x => x.Id);
        b.HasIndex(x => new { x.SectionId, x.Position }).IsUnique();
    }
}
