namespace Infrastructure.Persistence
{
    using Application.Common.DomainEvents;
    using Application.Persistence;
    using Domain.Common;
    using Domain.Todo;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext(DbContextOptions options,
            IDomainEventService domainEventService) : base(options)
        {
            _domainEventService = domainEventService;
        }

        public DbSet<TodoItem> TodoItems => Set<TodoItem>();

        public DbSet<TodoList> TodoLists => Set<TodoList>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        //entry.Entity.CreatedBy = _currentUserService.UserId;
                        entry.Entity.Created = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        //entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        entry.Entity.LastModified = DateTime.UtcNow;
                        break;
                }
            }

            await DispatchEvents();

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        private async Task DispatchEvents()
        {
            var domainEvents = ChangeTracker
                .Entries<IHaveDomainEvents>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await _domainEventService.Publish(domainEvent);
            }
        }
    }
}