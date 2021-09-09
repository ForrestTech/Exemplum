namespace Infrastructure.Persistence.Configurations
{
    using Domain.Audit;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AuditItemConfiguration : IEntityTypeConfiguration<AuditItem>
    {
        public void Configure(EntityTypeBuilder<AuditItem> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.Property(t => t.EventType)
                .HasMaxLength(500)
                .IsRequired();
            
            builder.Property(t => t.EventData)
                .IsRequired();
        }
    }
}