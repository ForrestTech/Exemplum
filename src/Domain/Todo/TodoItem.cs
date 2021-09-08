namespace Domain.Todo
{
    using Common;
    using System;

    public class TodoItem : BaseEntity
    {
        public TodoItem() { }
        
        public TodoItem(string title)
        {
            Title = title;
        }
        
        public int ListId { get; private set; }
        public TodoList List { get; private set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public PriorityLevel Priority { get; private set; }

        public DateTime? Reminder { get; private set; }
    
        public Colour Colour { get; private set; } = Colour.White;

        private bool _done;

        // Example of a property that can be set on init and also via behaviour on the entity
        public bool Done { get => _done; init => _done = value; }
        
        public void MarkAsDone()
        {
            _done = true;
        }
    }
}