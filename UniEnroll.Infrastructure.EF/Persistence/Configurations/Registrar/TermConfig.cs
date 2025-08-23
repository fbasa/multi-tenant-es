
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Registrar;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Registrar;

public sealed class TermConfig : IEntityTypeConfiguration<Term>
{
    public void Configure(EntityTypeBuilder<Term> b)
    {
        b.ToTable("Terms");
        b.HasKey(x => x.Id);
        b.Property(x => x.Status).HasMaxLength(32);
    }
}
