namespace Exemplum.Application.TodoList.Queries;

using Common.Mapping;
using Common.Pagination;
using Models;
using Persistence;

public class GetTodoListsQuery : 
    IRequest<OneOf<PaginatedList<TodoListDto>, ValidationFailed>>,
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

public class GetTodoListsQueryHandler : 
    IRequestHandler<GetTodoListsQuery, OneOf<PaginatedList<TodoListDto>, ValidationFailed>>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<GetTodoListsQuery> _validator;

    public GetTodoListsQueryHandler(IApplicationDbContext context,
        IValidator<GetTodoListsQuery> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<OneOf<PaginatedList<TodoListDto>, ValidationFailed>> Handle(GetTodoListsQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsInvalid())
        {
            return validationResult.ToFailure();
        }
        
        return await _context.TodoLists
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .Select(x => x.MapToDto())
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}