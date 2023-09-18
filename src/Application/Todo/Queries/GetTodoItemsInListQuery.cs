namespace Exemplum.Application.Todo.Queries;

using Models;

public class GetTodoItemsInListQuery : 
    IRequest<OneOf<PaginatedList<TodoItemDto>, ValidationFailed>>,
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

public class GetTodoItemsQueryHandler : IRequestHandler<GetTodoItemsInListQuery, OneOf<PaginatedList<TodoItemDto>, ValidationFailed>>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<GetTodoItemsInListQuery> _validator;

    public GetTodoItemsQueryHandler(IApplicationDbContext context, 
        IValidator<GetTodoItemsInListQuery> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<OneOf<PaginatedList<TodoItemDto>, ValidationFailed>> Handle(GetTodoItemsInListQuery request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (validation.IsInvalid())
        {
            return validation.ToFailure();
        }
        
        var list = await _context.TodoItems
            .AsNoTracking()
            .Where(x => x.ListId == request.ListId)
            .OrderBy(x => x.Title)
            .Select(x => x.MapToDto())
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

        return list;
    }
}