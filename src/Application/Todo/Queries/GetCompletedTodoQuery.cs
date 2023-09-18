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

public class GetCompletedTodoItemsQuery :
    IRequest<OneOf<PaginatedList<TodoItemDto>, ValidationFailed>>,
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

public class GetCompletedTodoQueryHandler : IRequestHandler<GetCompletedTodoItemsQuery, OneOf<PaginatedList<TodoItemDto>, ValidationFailed>>
{
    private readonly IApplicationDbContext _context;
    private readonly IValidator<GetCompletedTodoItemsQuery> _validator;

    public GetCompletedTodoQueryHandler(IApplicationDbContext context,
        IValidator<GetCompletedTodoItemsQuery> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<OneOf<PaginatedList<TodoItemDto>, ValidationFailed>> Handle(GetCompletedTodoItemsQuery request,
        CancellationToken cancellationToken)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (validation.IsInvalid())
        {
            return validation.ToFailure();
        }
        
        return await _context.TodoItems
            .AsNoTracking()
            .Query(request)
            .Select(x => x.MapToDto())
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}