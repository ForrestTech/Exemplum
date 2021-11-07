namespace Exemplum.Domain.Todo;

using Common;
using Events;

public class TodoList : BaseEntity, IAggregateRoot, ISoftDelete
{
    /// <summary>
    /// EF constructor
    /// </summary>
    private TodoList()
    {
    }

    public TodoList(string title)
    {
        Title = title;
    }

    public TodoList(string title, Colour colour)
    {
        Title = title;
        Colour = colour;
    }

    public string Title { get; set; } = string.Empty;

    public Colour? Colour { get; set; }

    private readonly List<TodoItem> _items = new();
    public IEnumerable<TodoItem> Items => _items.AsReadOnly();

    public void AddToDo(TodoItem item)
    {
        _items.Add(item);

        DomainEvents.Add(new TodoItemCreatedEvent(item));
    }

    public void AddToDo(List<TodoItem> items)
    {
        items.ForEach(AddToDo);
    }

    public bool IsDeleted { get; set; }
}