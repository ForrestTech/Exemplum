namespace Application.TodoList.Queries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Mapping;
    using Common.Pagination;
    using Common.Validation;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Persistence;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetTodoListsQuery : IRequest<PaginatedList<TodoListDto>>,
        IPaginatedQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    
    public class GetTodoListsQueryValidator : AbstractValidator<GetTodoListsQuery>
    {
        public GetTodoListsQueryValidator()
        {
            Include(new PaginatedQueryValidator());
        }
    }

    public class GetTodoListsQueryHandler : IRequestHandler<GetTodoListsQuery, PaginatedList<TodoListDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTodoListsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TodoListDto>> Handle(GetTodoListsQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.TodoLists
                .AsNoTracking()
                .OrderBy(x => x.Title)
                .ProjectTo<TodoListDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}