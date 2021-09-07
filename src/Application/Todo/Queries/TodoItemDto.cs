namespace Application.Todo.Queries
{
    using Common.Mapping;
    using Domain.Todo;

    public class TodoItemDto : IMapFrom<TodoItem>
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        public string Title { get; set; }

        public bool Done { get; set; }
    }
}