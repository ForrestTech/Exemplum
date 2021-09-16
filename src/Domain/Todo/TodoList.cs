namespace Exemplum.Domain.Todo
{
    using Common;
    using Events;
    using System.Collections.Generic;

    public class TodoList : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// EF constructor
        /// </summary>
        private TodoList()
        { }
        
        public TodoList(string title, Colour colour)
        {
            Title = title;
            Colour = colour;
        }

        public string Title { get; private set; } = string.Empty;

        public Colour Colour { get; private set; } = Colour.White;

        private readonly List<TodoItem> _items = new List<TodoItem>();
        public IEnumerable<TodoItem> Items => _items.AsReadOnly();

        public void AddToDo(TodoItem item)
        {
            _items.Add(item);

            DomainEvents.Add(new TodoItemCreatedEvent(item));
        }
        
        public void AddToDo(List<TodoItem> items)
        {
            items.ForEach(x =>
            {
                _items.Add(x);

                DomainEvents.Add(new TodoItemCreatedEvent(x));    
            });
        }
    }
}