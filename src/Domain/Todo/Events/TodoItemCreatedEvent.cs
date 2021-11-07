namespace Exemplum.Domain.Todo.Events;

using Common;

public class TodoItemCreatedEvent : DomainEvent
{
    public TodoItemCreatedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}