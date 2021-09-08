namespace Application.Persistence
{
    using Domain.Todo;
    using Microsoft.EntityFrameworkCore;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IApplicationDbContext
    {
        public DbSet<TodoList> TodoLists { get; }

        public DbSet<TodoItem> TodoItems { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}