namespace Application.TodoList.Commands
{
    using AutoMapper;
    using Domain.Todo;
    using FluentValidation;
    using MediatR;
    using Models;
    using Persistence;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateTodoListCommand : IRequest<TodoListDto>
    {
        public string Title { get; set; } = string.Empty;

        public string Colour { get; set; } = string.Empty;
    }

    public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
    {
        public CreateTodoListCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
            RuleFor(x => x.Colour).NotEmpty().Must(Colour.IsValidColour).WithMessage("{PropertyName} must be a valid colour not {PropertyValue}");
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
            var list = new TodoList(request.Title, Colour.From(request.Colour));

            _context.TodoLists.Add(list);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TodoListDto>(list);
        }
    }
}