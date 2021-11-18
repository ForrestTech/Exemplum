namespace Exemplum.Application.Common.IntegrationEvents
{
    using Domain.Common;
    using System.Threading.Tasks;

    /// <summary>
    /// Publishes integration events that are intended to be consumed by other services either inside or outside of the current bounded context.  
    /// </summary>
    public interface IIntegrationEventPublisher
    {
        Task PublishAsync<T>(T integrationEvent) where T : IntegrationEvents;
    }
}