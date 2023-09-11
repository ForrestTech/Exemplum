namespace Exemplum.Application.TodoList.Commands;

using Common.Exceptions;
using Common.Security;
using Domain.Todo;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence;

[Authorize(Policy = Security.Policy.CanWriteTodo)]
public class UpdateTodoListCommand : IRequest<TodoListDto>
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

public class UpdateTodoListCommandValidatorHandler : IRequestHandler<UpdateTodoListCommand, TodoListDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoListCommandValidatorHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoListDto> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var list = await _context.TodoLists
            .SingleOrDefaultAsync(x => x.Id == request.ListId, cancellationToken);

        if (list == null)
        {
            throw new NotFoundException();
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