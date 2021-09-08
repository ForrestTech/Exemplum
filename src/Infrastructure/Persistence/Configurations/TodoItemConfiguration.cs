namespace Infrastructure.Persistence.Configurations
{
    using Domain.Todo;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            
            builder.Property(t => t.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.Priority)
                .HasConversion(
                    x => x!.ToString(),
                    x => PriorityLevel.FromValue(x));
        }
    }
}