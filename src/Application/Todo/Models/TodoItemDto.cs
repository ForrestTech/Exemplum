namespace Exemplum.Application.Todo.Models;

using Domain.Todo;

public class TodoItemDto 
{
    public int Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Note { get; init; } = string.Empty;

    public string Priority { get; init; } = string.Empty;

    public bool Done { get; init; }
}

public static class TodoItemExtensions
{
    public static TodoItemDto MapToDto(this TodoItem item)
    {
        return new TodoItemDto
        {
            Id = item.Id,
            Title = item.Title,
            Note = item.Note,
            Priority = item.Priority?.Name ?? string.Empty,
            Done = item.Done
        };
    }
}