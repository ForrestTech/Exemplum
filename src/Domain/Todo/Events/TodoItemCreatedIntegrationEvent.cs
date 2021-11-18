namespace Exemplum.Domain.Todo.Events
{
    using Common;

    public class TodoItemCreatedIntegrationEvent : IntegrationEvents
    {
        public string Title { get; set; } = string.Empty;
    }
}