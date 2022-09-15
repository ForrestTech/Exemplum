namespace Exemplum.Application.TodoList.Commands;

using Common.Security;
using Domain.Exceptions;
using Domain.Todo;
using Models;
using System.Net;

[Authorize(Policy = Security.Policy.CanWriteTodo)]
public class CreateTodoListCommand : IRequest<TodoListDto>
{
    public string Title { get; set; } = string.Empty;

    public string? Colour { get; set; }
}

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    public CreateTodoListCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
            .MaximumLength(300)            
            .WithMessage("Todo list title must be unique");

        RuleFor(x => x.Colour)
            .Must(x => x == null || Colour.IsValidColour(x))
            .WithMessage("{PropertyName} must be a valid Colour");
    }    
}

public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, TodoListDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateTodoListCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TodoListDto> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var list = new TodoList(request.Title);

        if (request.Colour is not null)
        {
            list.Colour = Colour.From(request.Colour);
        }

        _context.TodoLists.Add(list);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TodoListDto>(list);
    }
}