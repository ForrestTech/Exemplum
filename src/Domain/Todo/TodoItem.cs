namespace Domain.Todo
{
    using Common;
    using Common.DateAndTime;
    using Events;
    using System;

    public class TodoItem : BaseEntity
    {
        public TodoItem(string title)
        {
            Title = title;
            Priority = PriorityLevel.None;
        }

        public int ListId { get; private set; }
        public TodoList List { get; set; } = null!;

        public string Title { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;

        public PriorityLevel? Priority { get; private set; }

        public DateTime? Reminder { get; private set; }

        private bool _done;
        
        // Example of a property that can be set on init and also via behaviour on the entity
        public bool Done { get => _done; init => _done = value; }

        public void MarkAsDone()
        {
            if (_done != false)
            {
                return;
            }

            _done = true;
            DomainEvents.Add(new TodoItemCompletedEvent(this));
        }

        /// <summary>
        /// This is a good example of using SmartEnum to hold behaviour in your model and also of double dispatch, injecting strategies into domains in this case a clock
        /// </summary>
        public void SetPriority(PriorityLevel priority, IClock clock)
        {
            Priority = priority;

            Reminder = clock.Now.Add(priority.ReminderTime);
        }
    }
}