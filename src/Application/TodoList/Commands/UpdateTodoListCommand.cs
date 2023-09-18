namespace Exemplum.Application.TodoList.Commands;

using Common.Security;
using Domain.Todo;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence;

[Authorize(Policy = Security.Policy.CanWriteTodo)]
public class UpdateTodoListCommand : 
    IRequest<OneOf<TodoListDto, NotFound, ValidationFailed>>
{
    public int ListId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Colour { get; set; }
}

public class UpdateTodoListCommandValidator : AbstractValidator<UpdateTodoListCommand>
{
    public UpdateTodoListCommandValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0);

        RuleFor(x => x.Title).NotEmpty()
            .MaximumLength(300)
            .WithMessage("Todo list title must be unique");

        RuleFor(x => x.Colour)
            .Must(x => x == null || Colour.IsValidColour(x))
            .WithMessage("{PropertyName} must be a valid colour not {PropertyValue}");
    }
}

public class UpdateTodoListCommandValidatorHandler :
    IRequestHandler<UpdateTodoListCommand, OneOf<TodoListDto, NotFound, ValidationFailed>>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<UpdateTodoListCommand> _validator;

    public UpdateTodoListCommandValidatorHandler(IApplicationDbContext context, 
        IValidator<UpdateTodoListCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<OneOf<TodoListDto, NotFound, ValidationFailed>> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsInvalid())
        {
            return validationResult.ToFailure();
        }
        
        var list = await _context.TodoLists
            .SingleOrDefaultAsync(x => x.Id == request.ListId, cancellationToken);

        if (list is null)
        {
            return new NotFound();
        }

        list.Title = request.Title;

        if (request.Colour is not null)
        {
            list.Colour = Colour.From(request.Colour);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return list.MapToDto();
    }
}