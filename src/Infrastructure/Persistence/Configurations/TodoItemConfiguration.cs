namespace Exemplum.Infrastructure.Persistence.Configurations;

using Domain.Todo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.Property(t => t.Title)
            .IsRequired();

        builder.Property(t => t.Note);

        builder.Property(t => t.Priority)
            .HasConversion(
                x => x!.ToString(),
                x => PriorityLevel.Parse(x));
    }
}