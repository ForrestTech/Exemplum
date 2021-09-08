namespace Application.Todo.Models
{
    using Common.Mapping;
    using Domain.Todo;

    public class TodoItemDto : IMapFrom<TodoItem>
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;

        public bool Done { get; set; }
    }
}