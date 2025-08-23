
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Registrar;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Registrar;

public sealed class StudentHoldConfig : IEntityTypeConfiguration<StudentHold>
{
    public void Configure(EntityTypeBuilder<StudentHold> b)
    {
        b.ToTable("StudentHolds");
        b.HasKey(x => x.Id);
        b.HasIndex(x => new { x.StudentId, x.HoldType }).IsUnique();
    }
}
