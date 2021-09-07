namespace Application.Todo.Queries
{
    using Common.Mapping;
    using Domain.Todo;
    using MediatR;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetTodoItemsQuery : IRequest<List<TodoItemDto>>
    {
        public int ListId { get; set; }
    }

    public class TodoItemDto : IMapFrom<TodoItem>
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        public string Title { get; set; }

        public bool Done { get; set; }
    }

    public class GetTodoQueryHandler : IRequestHandler<GetTodoItemsQuery, List<TodoItemDto>>
    {
        public Task<List<TodoItemDto>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new List<TodoItemDto> { new TodoItemDto { Id = 1, ListId = 2, Title = "Something" } });
        }
    }
}