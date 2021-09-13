namespace Infrastructure.Persistence.Configurations
{
    using Domain.Todo;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {
            builder.Property(x => x.Title)
                .HasMaxLength(300)
                .IsRequired();

            builder.HasIndex(x => x.Title)
                .IsUnique();

            builder.Property(x => x.Colour)
                .HasConversion(
                    x => x!.ToString(),
                    x => Colour.From(x));
        }
    }
}