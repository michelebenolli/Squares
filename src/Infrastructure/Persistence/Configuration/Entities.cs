using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Squares.Infrastructure.Auditing;

namespace Squares.Infrastructure.Persistence.Configuration;
public class TrailConfig : IEntityTypeConfiguration<Trail>
{
    public void Configure(EntityTypeBuilder<Trail> builder) =>
        builder
            .ToTable("Trail", SchemaNames.Auditing)
            .IsMultiTenant();
}
