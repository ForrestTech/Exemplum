namespace Exemplum.Infrastructure.IntegrationEvents
{
    using Application.Common.IntegrationEvents;
    using Domain.Common;
    using MassTransit;
    using System.Threading.Tasks;

    public class IntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public IntegrationEventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T integrationEvent) where T : IntegrationEvents
        {
            await _publishEndpoint.Publish(integrationEvent);
        }
    }
}