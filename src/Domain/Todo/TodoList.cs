namespace Domain.Todo
{
    using Common;
    using Events;
    using System.Collections.Generic;

    public class TodoList : BaseEntity, IAggregateRoot
    {
        public TodoList() { }

        public TodoList(List<TodoItem> items)
        {
            _items = items;
        }
        
        public string Title { get; set; }

        public Colour Colour { get; set; } = Colour.White;

        private readonly List<TodoItem> _items = new List<TodoItem>();
        public IEnumerable<TodoItem> Items => _items.AsReadOnly();

        public void AddToDo(TodoItem item)
        {
            _items.Add(item);

            DomainEvents.Add(new TodoItemCreated(item));
        }
    }
}