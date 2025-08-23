using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniEnroll.Domain.Courses;
using UniEnroll.Domain.Courses.ValueObjects;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations;

internal sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> b)
    {
        b.ToTable("Courses");
        b.HasKey(x => x.Id);
        b.Property(x => x.TenantId).HasMaxLength(64).IsRequired();
        b.HasIndex(x => new { x.TenantId, Code = x.Code }).IsUnique();

        var codeConv = new ValueConverter<CourseCode, string>(v => v.Value, v => new CourseCode(v ?? string.Empty));
        var unitConv = new ValueConverter<CreditUnit, int>(v => v.Value, v => new CreditUnit(v));

        b.Property(x => x.Code).HasConversion(codeConv).HasMaxLength(32).IsRequired();
        b.Property(x => x.Title).HasMaxLength(256).IsRequired();
        b.Property(x => x.Units).HasConversion(unitConv).IsRequired();
    }
}
