namespace Exemplum.Infrastructure.DomainEvents
{
    using Application.Common.DomainEvents;
    using Domain.Common;
    using System.Threading.Tasks;

    public class NoOpDomainEventsPublisher : IPublishDomainEvents
    {
        public Task Publish(DomainEvent domainEvent)
        {
            return Task.CompletedTask;
        }
    }
}