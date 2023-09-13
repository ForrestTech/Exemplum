namespace Exemplum.Infrastructure.Persistence.Configurations;

using Domain.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AuditItemConfiguration : IEntityTypeConfiguration<AuditItem>
{
    public void Configure(EntityTypeBuilder<AuditItem> builder)
    {
        builder.Property(t => t.EventType)
            .IsRequired();

        builder.Property(t => t.EventData)
            .IsRequired();
    }
}