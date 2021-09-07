namespace Domain.Todo.Events
{
    using Common;

    public class TodoItemCreated: DomainEvent
    {
        public TodoItemCreated(TodoItem item)
        {
            Item = item;
        }

        public TodoItem Item { get; }
    }
}