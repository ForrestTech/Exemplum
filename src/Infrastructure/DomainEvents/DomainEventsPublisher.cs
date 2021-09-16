namespace Exemplum.Infrastructure.DomainEvents
{
    using Application.Common.DomainEvents;
    using Domain.Common;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class DomainEventsPublisher : IPublishDomainEvents
    {
        private readonly ILogger<DomainEventsPublisher> _logger;
        private readonly IPublisher _mediator;

        public DomainEventsPublisher(ILogger<DomainEventsPublisher> logger, IPublisher mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Publish(DomainEvent domainEvent)
        {
            _logger.LogInformation("Publishing domain event. Event - {Event}", domainEvent.GetType().Name);
            
            var notification = GetNotificationCorrespondingToDomainEvent(domainEvent);

            if (notification != null)
            {
                await _mediator.Publish(notification);
            }
        }

        private static INotification? GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
        {
            var instance = Activator.CreateInstance(typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
            if (instance != null)
            {
                INotification? notification = (INotification)instance;
                return notification;
            }

            return null;
        }
    }
}