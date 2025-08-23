using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations;

internal sealed class ApplicationConfiguration : IEntityTypeConfiguration<UniEnroll.Domain.Admissions.Application>
{
    public void Configure(EntityTypeBuilder<UniEnroll.Domain.Admissions.Application> b)
    {
        b.ToTable("Applications");
        b.HasKey(x => x.Id);
        b.Property(x => x.TenantId).HasMaxLength(64).IsRequired();
        b.Property(x => x.ProgramId).HasMaxLength(64).IsRequired();
        b.Property(x => x.TermCode).HasMaxLength(16).IsRequired();
        b.Property(x => x.Status).HasMaxLength(32).IsRequired();
    }
}
