namespace Exemplum.Domain.Todo;

using Common;
using Common.DateAndTime;
using Events;

public class TodoItem : BaseEntity
{
    private bool _done;

    public TodoItem(string title)
    {
        Title = title;
        Priority = PriorityLevel.None;
    }

    public int ListId { get; private set; }
    public TodoList List { get; set; } = null!;

    public string Title { get; set; }

    public string Note { get; set; } = string.Empty;

    public PriorityLevel Priority { get; private set; }

    public DateTime? Reminder { get; private set; }


    // Example of a property that can be set on init and also via behaviour on the entity, this format makes it easier to setup test data but also protect invariance 
    public bool Done { get => _done; init => _done = value; }

    public void MarkAsDone()
    {
        if (_done)
        {
            return;
        }

        _done = true;
        DomainEvents.Add(new TodoItemCompletedEvent(this));
    }

    public void MarkAsIncomplete()
    {
        _done = false;
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