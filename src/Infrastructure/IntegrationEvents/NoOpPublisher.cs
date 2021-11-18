namespace Exemplum.Infrastructure.IntegrationEvents;

using Application.Common.IntegrationEvents;
using Domain.Common;

public class NoOpPublisher : IIntegrationEventPublisher
{
    public Task PublishAsync<T>(T integrationEvent) where T : IntegrationEvents
    {
        return Task.CompletedTask;
    }
}