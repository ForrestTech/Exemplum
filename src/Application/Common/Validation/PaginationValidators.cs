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
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber at least greater than or equal to 1.");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize at least greater than or equal to 1.");
        }
    }
}