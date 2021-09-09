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
    using Persistence;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetTodoListTodoItemsQuery : IRequest<PaginatedList<TodoItemDto>>, 
        IPaginatedQuery
    {
        public int ListId { get; set; }
        
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    
    public class GetTodoItemsQueryValidator : AbstractValidator<GetTodoListTodoItemsQuery>
    {
        public GetTodoItemsQueryValidator()
        {
            RuleFor(x => x.ListId).GreaterThan(0);
            
            RuleFor(x => x.PageNumber).ValidPageNumber(1);

            RuleFor(x => x.PageSize).ValidPageSize(1);
        }
    }

    public class GetTodoItemsQueryHandler : IRequestHandler<GetTodoListTodoItemsQuery, PaginatedList<TodoItemDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetTodoItemsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<PaginatedList<TodoItemDto>> Handle(GetTodoListTodoItemsQuery request, CancellationToken cancellationToken)
        {
            return await _context.TodoItems
                .AsNoTracking()
                .Where(x => x.ListId == request.ListId)
                .OrderBy(x => x.Title)
                .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}