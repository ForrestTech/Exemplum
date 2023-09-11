namespace Exemplum.Application.TodoList.Queries;

using Common.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence;

public class GetTodoListByIdQuery : IRequest<TodoListDto>
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

public class GetTodoListQueryHandler : IRequestHandler<GetTodoListByIdQuery, TodoListDto>
{
    private readonly IApplicationDbContext _context;

    public GetTodoListQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoListDto> Handle(GetTodoListByIdQuery request,
        CancellationToken cancellationToken)
    {
        var todoList = await _context.TodoLists
            .AsNoTracking()
            .Where(x => x.Id == request.ListId)
            .Select(x => x.MapToDto())
            .SingleOrDefaultAsync(cancellationToken);

        if (todoList == null)
        {
            throw new NotFoundException(nameof(TodoList), new {listId = request.ListId});
        }

        return todoList;
    }
}