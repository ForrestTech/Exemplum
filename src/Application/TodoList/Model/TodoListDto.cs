namespace Application.TodoList.Model
{
    using Common.Mapping;
    using Domain.Todo;

    public class TodoListDto : IMapFrom<TodoList>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}