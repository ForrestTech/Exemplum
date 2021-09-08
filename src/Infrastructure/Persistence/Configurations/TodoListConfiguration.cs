namespace Infrastructure.Persistence.Configurations
{
    using Domain.Audit;
    using Domain.Todo;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            
            builder.Property(t => t.Title)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(t => t.Colour)
                .HasConversion(
                    x => x!.ToString(),
                    x => Colour.From(x));
        }
    }
    
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