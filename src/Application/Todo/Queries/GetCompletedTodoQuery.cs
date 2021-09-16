namespace Exemplum.Application.Todo.Queries
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Mapping;
    using Common.Pagination;
    using Common.Validation;
    using Domain.Todo;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Persistence;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class GetCompletedTodoItemsQuery : IRequest<PaginatedList<TodoItemDto>>, 
        IPaginatedQuery,
        IQueryObject<TodoItem>
    {
        public int ListId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public IQueryable<TodoItem> ApplyQuery(IQueryable<TodoItem> query)
        {
            query = query.Where(x => x.ListId == ListId 
                                     && x.Done)
                .OrderBy(x => x.Title);

            return query;
        }
    }

    public class GetCompletedTodoItemsQueryValidator : AbstractValidator<GetCompletedTodoItemsQuery>
    {
        public GetCompletedTodoItemsQueryValidator()
        {
            RuleFor(x => x.ListId).GreaterThan(0);
            Include(new PaginatedQueryValidator());
        }
    }

    public class GetCompletedTodoQueryHandler : IRequestHandler<GetCompletedTodoItemsQuery, PaginatedList<TodoItemDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCompletedTodoQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TodoItemDto>> Handle(GetCompletedTodoItemsQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.TodoItems
                .AsNoTracking()
                .Query(request)
                .ProjectTo<TodoItemDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
        }
    }
}