namespace Exemplum.Application.TodoList.Queries;

using Models;
using Persistence;

public class GetTodoListByIdQuery : 
    IRequest<OneOf<TodoListDto, NotFound, ValidationFailed>>
{
    public int ListId { get; set; }
}

public class GetTodoListQueryValidator : AbstractValidator<GetTodoListByIdQuery>
{
    public GetTodoListQueryValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0);
    }
}

public class GetTodoListQueryHandler : 
    IRequestHandler<GetTodoListByIdQuery, OneOf<TodoListDto, NotFound, ValidationFailed>>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<GetTodoListByIdQuery> _validator;

    public GetTodoListQueryHandler(IApplicationDbContext context, 
        IValidator<GetTodoListByIdQuery> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<OneOf<TodoListDto, NotFound, ValidationFailed>> Handle(GetTodoListByIdQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsInvalid())
        {
            return validationResult.ToFailure();
        }
        
        var todoList = await _context.TodoLists
            .AsNoTracking()
            .Where(x => x.Id == request.ListId)
            .Select(x => x.MapToDto())
            .SingleOrDefaultAsync(cancellationToken);

        if (todoList == null)
        {
            return new NotFound();
        }

        return todoList;
    }
}