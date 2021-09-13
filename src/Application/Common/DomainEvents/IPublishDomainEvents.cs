namespace Application.Common.DomainEvents
{
    using Domain.Common;
    using System.Threading.Tasks;

    public interface IPublishDomainEvents
    {
        Task Publish(DomainEvent domainEvent);
    }
}