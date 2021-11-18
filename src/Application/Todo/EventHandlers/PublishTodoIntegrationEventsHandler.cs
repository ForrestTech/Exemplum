namespace Exemplum.Application.Todo.EventHandlers
{
    using Common.DomainEvents;
    using Common.IntegrationEvents;
    using Domain.Todo.Events;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public class PublishTodoIntegrationEventsHandler : INotificationHandler<DomainEventNotification<TodoItemCreatedEvent>>
    {
        private readonly IIntegrationEventPublisher _integrationEventPublisher;

        public PublishTodoIntegrationEventsHandler(IIntegrationEventPublisher integrationEventPublisher)
        {
            _integrationEventPublisher = integrationEventPublisher;
        }
        
        public async Task Handle(DomainEventNotification<TodoItemCreatedEvent> notification, CancellationToken cancellationToken)
        {
            await _integrationEventPublisher.PublishAsync(new TodoItemCreatedIntegrationEvent
            {
                Title = notification.DomainEvent.Item.Title
            });
        }
    }
}