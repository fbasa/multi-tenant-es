
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Students;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Students;

public sealed class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> b)
    {
        b.ToTable("Students");
        b.HasKey(x => x.Id);
        b.Property(x => x.Email).HasMaxLength(256).IsRequired();
        b.Property(x => x.ProgramId).HasMaxLength(64).IsRequired();
        b.Property(x => x.TenantId).HasMaxLength(64).IsRequired();
    }
}
