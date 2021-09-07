namespace Application.Todo.Queries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Mapping;
    using Common.Pagination;
    using Domain.Todo;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetTodoItemsWithPaginationQuery : IRequest<PaginatedList<TodoItemDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class TodoItemDto : IMapFrom<TodoItem>
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        public string Title { get; set; }

        public bool Done { get; set; }
    }

    public class GetTodoQueryHandler : IRequestHandler<GetTodoItemsWithPaginationQuery, PaginatedList<TodoItemDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTodoQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PaginatedList<TodoItemDto>> Handle(GetTodoItemsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.TodoItems
                .AsNoTracking()
                .OrderBy(x => x.Title)
                .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}