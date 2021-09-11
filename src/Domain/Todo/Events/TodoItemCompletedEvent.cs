namespace Domain.Todo.Events
{
    using Common;

    public class TodoItemCompletedEvent : DomainEvent
    {
        public TodoItem TodoItem { get; }

        public TodoItemCompletedEvent(TodoItem todoItem)
        {
            TodoItem = todoItem;
        }
    }
}