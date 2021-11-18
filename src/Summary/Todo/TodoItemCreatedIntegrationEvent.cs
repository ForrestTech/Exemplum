namespace Exemplum.Domain.Todo.Events
{
    /// <summary>
    /// For mass transit to automatically map events the full type and namespace needs to match.  You could publish a contracts package that is shared by producers and consumers however, this does
    /// couple services together more than a simple copy paste.
    /// </summary>
    public class TodoItemCreatedIntegrationEvent
    {
        public string Title { get; set; } = string.Empty;

        public int ListId { get; set; }
    }
}