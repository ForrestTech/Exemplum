namespace Application.Persistence
{
    using Domain.Audit;
    using Domain.Todo;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// This DB Context should be used from all domain event handlers as it does not support direct saving.
    /// This is to make sure all updates happen in the scope of a single transaction
    /// </summary>
    public interface IEventHandlerDbContext
    {
        public DbSet<TodoList> TodoLists { get; }

        public DbSet<TodoItem> TodoItems { get; }
        
        public DbSet<AuditItem> AuditItems { get; }
    }
}