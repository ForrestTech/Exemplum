namespace Exemplum.Application.Todo.Queries;

using Common.Mapping;
using Common.Pagination;
using Common.Validation;
using Domain.Todo;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Persistence;

public class GetCompletedTodoItemsQuery : IRequest<PaginatedList<TodoItemDto>>,
    IPaginatedQuery,
    IQueryObject<TodoItem>
{
    public int ListId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public IQueryable<TodoItem> ApplyQuery(IQueryable<TodoItem> query)
    {
        query = query.Where(x => x.ListId == ListId
                                 && x.Done)
            .OrderBy(x => x.Title);

        return query;
    }
}

public class GetCompletedTodoItemsQueryValidator : AbstractValidator<GetCompletedTodoItemsQuery>
{
    public GetCompletedTodoItemsQueryValidator()
    {
        RuleFor(x => x.ListId).GreaterThan(0);
        Include(new PaginatedQueryValidator());
    }
}

public class GetCompletedTodoQueryHandler : IRequestHandler<GetCompletedTodoItemsQuery, PaginatedList<TodoItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCompletedTodoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<TodoItemDto>> Handle(GetCompletedTodoItemsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.TodoItems
            .AsNoTracking()
            .Query(request)
            .Select(x => x.MapToDto())
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}