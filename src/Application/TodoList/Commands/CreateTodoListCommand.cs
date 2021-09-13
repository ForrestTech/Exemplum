namespace Application.TodoList.Commands
{
    using AutoMapper;
    using Domain.Todo;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
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
        private readonly IApplicationDbContext _context;

        public CreateTodoListCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            
            RuleFor(x => x.Title).NotEmpty()
                .MaximumLength(300)
                .MustAsync(BeUniqueTitle)
                .WithMessage("Todo list title must be unique");
            
            RuleFor(x => x.Colour).NotEmpty()
                .Must(Colour.IsValidColour)
                .WithMessage("{PropertyName} must be a valid colour not {PropertyValue}");
        }


        private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
        {
            return await _context.TodoLists
                .AllAsync(l => l.Title != title, cancellationToken: cancellationToken);
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