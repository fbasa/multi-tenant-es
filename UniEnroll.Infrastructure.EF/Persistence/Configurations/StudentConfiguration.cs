using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniEnroll.Domain.Students;
using UniEnroll.Domain.Students.ValueObjects;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations;

internal sealed class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> b)
    {
        b.ToTable("Students");
        b.HasKey(x => x.Id);
        b.Property(x => x.TenantId).HasMaxLength(64).IsRequired();
        b.Property(x => x.Email).HasMaxLength(256).IsRequired();
        b.Property(x => x.ProgramId).HasMaxLength(64).IsRequired();
        b.Property(x => x.EntryYear).IsRequired();

        //var nameConv = new ValueConverter<StudentName, string>(
        //    v => $"{v.First}|{v.Last}",
        //    v => {
        //        var parts = (v ?? string.Empty).Split('|');
        //        var first = parts.Length > 0 ? parts[0] : string.Empty;
        //        var last = parts.Length > 1 ? parts[1] : string.Empty;
        //        return new StudentName(first, last);
        //    });
        b.Property(x => x.Name)
            //.HasConversion(nameConv)
            .HasMaxLength(256).IsRequired();
    }
}
