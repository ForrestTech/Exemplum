namespace Infrastructure.Persistence.Configurations
{
    using Domain.Todo;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            
            builder.Property(t => t.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.OwnsOne(b => b.Colour);
        }
    }
}