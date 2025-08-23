
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Instructors;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Instructors;

public sealed class InstructorConfig : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> b)
    {
        b.ToTable("Instructors");
        b.HasKey(x => x.Id);
        b.Property(x => x.Email).HasMaxLength(256);
        b.Property(x => x.TenantId).HasMaxLength(64);
    }
}
