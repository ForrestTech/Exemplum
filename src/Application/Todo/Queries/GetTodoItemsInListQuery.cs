namespace Exemplum.Application.Todo.Queries;

using Models;

public class GetTodoItemsInListQuery : IRequest<PaginatedList<TodoItemDto>>,
    IPaginatedQuery
{
    [JsonIgnore]
    public int ListId { get; set; }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetTodoItemsQueryValidator : AbstractValidator<GetTodoItemsInListQuery>
{
    public GetTodoItemsQueryValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0);
        Include(new PaginatedQueryValidator());
    }
}

public class GetTodoItemsQueryHandler : IRequestHandler<GetTodoItemsInListQuery, PaginatedList<TodoItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTodoItemsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<TodoItemDto>> Handle(GetTodoItemsInListQuery request,
        CancellationToken cancellationToken)
    {
        var list = await _context.TodoItems
            .AsNoTracking()
            .Where(x => x.ListId == request.ListId)
            .OrderBy(x => x.Title)
            .Select(x => x.MapToDto())
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

        return list;
    }
}