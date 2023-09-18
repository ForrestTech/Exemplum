namespace Exemplum.Application.Common.Pagination;

/// <summary>
/// Example of reusable fluent validation rules 
/// </summary>
public class PaginatedQueryValidator : AbstractValidator<IPaginatedQuery>
{
    public PaginatedQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1)
            .WithMessage("{PropertyName} at least greater than or equal to 1.");
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1)
            .WithMessage("{PropertyName} at least greater than or equal to 1.");
    }
}