
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using UniEnroll.Domain.Registrar;

namespace UniEnroll.Infrastructure.EF.Persistence.Configurations.Registrar;

public sealed class DegreeProgramConfig : IEntityTypeConfiguration<DegreeProgram>
{
    public void Configure(EntityTypeBuilder<DegreeProgram> b)
    {
        b.ToTable("DegreePrograms");
        b.HasKey(x => x.Id);
    }
}
