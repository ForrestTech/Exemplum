namespace Application.Todo.EventHandlers
{
    using Common.DomainEvents;
    using Domain.Audit;
    using Domain.Todo.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Persistence;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Simple domain event handler that demonstrates an audit system. This is not really a suggestion on how to implement this but more of a demo of having
    /// a single updates trigger domain events that cause updates to multiple aggregates this all happens in a single transaction by default
    /// </summary>
    public class TodoItemCreatedEventHandler : INotificationHandler<DomainEventNotification<TodoItemCreatedEvent>>
    {
        private readonly IEventHandlerDbContext _context;
        private readonly ILogger<TodoItemCreatedEventHandler> _logger;

        public TodoItemCreatedEventHandler(IEventHandlerDbContext context, 
            ILogger<TodoItemCreatedEventHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<TodoItemCreatedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            _logger.LogInformation("Domain Event Handled: {DomainEvent}", domainEvent.GetType().Name);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };
            
            _context.AuditItems.Add(new AuditItem
            {
                EventType = domainEvent.GetType().Name,
                EventData = System.Text.Json.JsonSerializer.Serialize(domainEvent.Item, options)
            });

            return Task.CompletedTask;
        }
    }
}