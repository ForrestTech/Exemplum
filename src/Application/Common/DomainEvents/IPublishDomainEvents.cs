namespace Exemplum.Application.Common.DomainEvents;

using Domain.Common;

public interface IPublishDomainEvents
{
    Task Publish(DomainEvent domainEvent);
}