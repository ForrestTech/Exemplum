namespace Infrastructure.Persistence.Configurations
{
    using Domain.Common;
    using Domain.Todo;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.Property(t => t.Title)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(t => t.Note)
                .HasMaxLength(2000);

            builder.Property(t => t.Priority)
                .HasConversion(
                    x => x!.ToString(),
                    x => PriorityLevel.FromValue(x));
        }
    }
}