namespace Exemplum.Infrastructure.Persistence.Configurations;

using Domain.Todo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.Property(x => x.Title)
            .IsRequired();

        builder.HasIndex(x => x.Title)
            .IsUnique();

        builder.Property(x => x.Colour)
            .HasConversion(
                x => x.Code.ToString(),
                x => Colour.From(x));
    }
}