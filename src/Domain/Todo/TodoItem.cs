namespace Domain.Todo
{
    using Common;
    using System;

    public class TodoItem : BaseEntity
    {
        public TodoItem(string title)
        {
            Title = title;
        }
        
        public string Title { get; set; }

        public string Note { get; set; }

        public PriorityLevel Priority { get; private set; }

        public DateTime? Reminder { get; private set; }
    
        public Colour Colour { get; private set; } = Colour.White;

        public bool Done { get; private set; }

        public void MarkAsDone()
        {
            Done = true;
        }
    }
}