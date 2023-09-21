namespace Exemplum.Application.Todo.Commands;

using Common.Security;
using Domain.Todo;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence;

[Authorize(Policy = Security.Policy.CanWriteTodo)]
public class CreateTodoItemCommand : 
    IRequest<OneOf<TodoItemDto, NotFound, ValidationFailed>>
{
    [JsonIgnore]
    public int ListId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Note { get; set; } = string.Empty;
}

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty()
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.Note).MaximumLength(2000);
    }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, 
    OneOf<TodoItemDto, NotFound, ValidationFailed>>
{
    private readonly IValidator<CreateTodoItemCommand> _validator;
    private readonly IApplicationDbContext _context;

    public CreateTodoItemCommandHandler(IValidator<CreateTodoItemCommand> validator,
        IApplicationDbContext context)
    {
        _validator = validator;
        _context = context;
    }

    public async Task<OneOf<TodoItemDto, NotFound, ValidationFailed>> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (validation.IsInvalid())
        {
            return validation.ToFailure();
        }
        
        var aggregate = await _context.TodoLists
            .Where(x => x.Id == request.ListId)
            .SingleOrDefaultAsync(cancellationToken);

        if (aggregate == null)
        {
            return new NotFound();
        }

        var entity = new TodoItem(request.Title) {Note = request.Note};

        aggregate.AddToDo(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.MapToDto();
    }
}