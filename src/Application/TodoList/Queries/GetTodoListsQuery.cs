namespace Exemplum.Application.TodoList.Queries;

using Common.Mapping;
using Common.Pagination;
using Common.Validation;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence;

public class GetTodoListsQuery : IRequest<PaginatedList<TodoListDto>>,
    IPaginatedQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetTodoListsQueryValidator : AbstractValidator<GetTodoListsQuery>
{
    public GetTodoListsQueryValidator()
    {
        Include(new PaginatedQueryValidator());
    }
}

public class GetTodoListsQueryHandler : IRequestHandler<GetTodoListsQuery, PaginatedList<TodoListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTodoListsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<TodoListDto>> Handle(GetTodoListsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.TodoLists
            .AsNoTracking()
            .OrderBy(x => x.Title)
            .Select(x => x.MapToDto())
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}