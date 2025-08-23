
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Admissions;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Admissions;

public sealed class ApplicationConfig : IEntityTypeConfiguration<Domain.Admissions.Application>
{
    public void Configure(EntityTypeBuilder<UniEnroll.Domain.Admissions.Application> b)
    {
        b.ToTable("Applications");
        b.HasKey(x => x.Id);
        b.Property<string>("Status").HasMaxLength(32);
        b.Property(x => x.TenantId).HasMaxLength(64);
    }
}
