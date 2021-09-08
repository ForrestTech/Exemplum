namespace Application.TodoList.Commands
{
    using AutoMapper;
    using Domain.Todo;
    using FluentValidation;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Todo.Models;

    public class CreateTodoItemCommand : IRequest<TodoItemDto>
    {
        public int ListId { get; set; }

        public string Title { get; set; } = string.Empty;
        
        public string Note { get; set; } = string.Empty;
    }

    public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
    {
        public CreateTodoItemCommandValidator()
        {
            RuleFor(x => x.ListId).GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty();
        }
    }

    public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, TodoItemDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateTodoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TodoItemDto> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await _context.TodoLists
                .Where(x => x.Id == request.ListId)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);

            var entity = new TodoItem(request.Title)
            {
                Note = request.Note
            };
            
            aggregate.AddToDo(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TodoItemDto>(entity);
        }
    }
}