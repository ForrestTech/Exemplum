namespace Exemplum.Application.Todo.Commands;

using Common.Security;
using Domain.Todo;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

[Authorize(Policy = Security.Policy.CanWriteTodo)]
public class UpdateTodoCommand :
    IRequest<OneOf<TodoItem, NotFound, ValidationFailed>>
{
    [JsonIgnore]
    public int ListId { get; set; }

    [JsonIgnore]
    public int TodoId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Note { get; set; } = string.Empty;
}

public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoCommandValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0);
        RuleFor(x => x.TodoId).GreaterThan(0);
    }
}

public class UpdateTodoCommandHandler :
    IRequestHandler<UpdateTodoCommand, OneOf<TodoItem, NotFound, ValidationFailed>>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<UpdateTodoCommand> _validator;

    public UpdateTodoCommandHandler(IApplicationDbContext context, IValidator<UpdateTodoCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<OneOf<TodoItem, NotFound, ValidationFailed>> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (validation.IsInvalid())
        {
            return validation.ToFailure();
        }
        
        var todo = await _context.TodoItems
            .Where(x => x.ListId == request.ListId && x.Id == request.TodoId)
            .SingleOrDefaultAsync(cancellationToken);

        if (todo == null)
        {
            return new NotFound();
        }

        todo.Title = request.Title;
        todo.Note = request.Note;

        await _context.SaveChangesAsync(cancellationToken);

        return todo;
    }
}