namespace Exemplum.Application.Todo.Queries;

using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence;

public class GetTodoItemByIdQuery :
    IRequest<OneOf<TodoItemDto, NotFound, ValidationFailed>>
{
    public int ListId { get; set; }

    public int TodoId { get; set; }
}

public class GetTodoItemInListQueryValidator : AbstractValidator<GetTodoItemByIdQuery>
{
    public GetTodoItemInListQueryValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0);

        RuleFor(x => x.TodoId).GreaterThan(0);
    }
}

public class GetTodoItemInListQueryHandler : IRequestHandler<GetTodoItemByIdQuery, OneOf<TodoItemDto, NotFound, ValidationFailed>>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<GetTodoItemByIdQuery> _validator;

    public GetTodoItemInListQueryHandler(IApplicationDbContext context,
        IValidator<GetTodoItemByIdQuery> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<OneOf<TodoItemDto, NotFound, ValidationFailed>> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (validation.IsInvalid())
        {
            return validation.ToFailure();
        }
        
        var todo = await _context.TodoItems
            .AsNoTracking()
            .Where(x => x.ListId == request.ListId && x.Id == request.TodoId)
            .Select(x => x.MapToDto())
            .SingleOrDefaultAsync(cancellationToken);

        if (todo == null)
        {
            return new NotFound();
        }

        return todo;
    }
}