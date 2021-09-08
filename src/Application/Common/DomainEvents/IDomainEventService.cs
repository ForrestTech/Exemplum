namespace Application.Common.DomainEvents
{
    using Domain.Common;
    using System.Threading.Tasks;

    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}