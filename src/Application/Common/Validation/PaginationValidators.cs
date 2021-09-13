namespace Application.Common.Validation
{
    using FluentValidation;
    using FluentValidation.Validators;
    using Pagination;
    using System;

    /// <summary>
    /// Example of reusable fluent validation rules 
    /// </summary>
    public class PaginatedQueryValidator : AbstractValidator<IPaginatedQuery>
    {
        public PaginatedQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("{PropertyName} at least greater than or equal to 1.");
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1).WithMessage("{PropertyName} at least greater than or equal to 1.");
        }
    }
}