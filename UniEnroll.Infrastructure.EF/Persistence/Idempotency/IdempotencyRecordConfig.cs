
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniEnroll.Infrastructure.EF.Persistence.Idempotency;

public sealed class IdempotencyRecordConfig : IEntityTypeConfiguration<IdempotencyRecord>
{
    public void Configure(EntityTypeBuilder<IdempotencyRecord> b)
    {
        b.ToTable("Idempotency");
        b.HasKey(x => x.Key);
        b.Property(x => x.Hash).HasMaxLength(128);
        b.HasIndex(x => x.ExpiresAt);
    }
}
