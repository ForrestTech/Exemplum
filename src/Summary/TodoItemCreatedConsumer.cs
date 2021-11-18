namespace Summary
{
    using Exemplum.Domain.Todo.Events;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class TodoItemCreatedConsumer : IConsumer<TodoItemCreatedIntegrationEvent>
    {
        readonly ILogger<TodoItemCreatedConsumer> _logger;

        public TodoItemCreatedConsumer(ILogger<TodoItemCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<TodoItemCreatedIntegrationEvent> context)
        {
            _logger.LogInformation("Received Todo: {Text}", context.Message.Title);

            return Task.CompletedTask;
        }
    }
}

namespace Exemplum.Domain.Todo.Events
{
    /// <summary>
    /// For mass transit to automatically map events the full type and namespace needs to match.  You could publish a contracts package that is shared by producers and consumers however, this does
    /// couple services together more than a simple copy paste.
    /// </summary>
    public class TodoItemCreatedIntegrationEvent
    {
        public string Title { get; set; } = string.Empty;
    }
}