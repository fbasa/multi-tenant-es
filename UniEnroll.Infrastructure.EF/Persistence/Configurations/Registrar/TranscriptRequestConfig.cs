
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Registrar;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Registrar;

public sealed class TranscriptRequestConfig : IEntityTypeConfiguration<TranscriptRequest>
{
    public void Configure(EntityTypeBuilder<TranscriptRequest> b)
    {
        b.ToTable("TranscriptRequests");
        b.HasKey(x => x.Id);
        b.HasIndex(x => x.StudentId);
    }
}
