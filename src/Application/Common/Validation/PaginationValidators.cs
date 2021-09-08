namespace Application.Common.Validation
{
    using FluentValidation;
    using FluentValidation.Validators;
    using System;

    /// <summary>
    /// Example of reusable fluent validation rules 
    /// </summary>
    public static class PaginationValidators
    {
        public static IRuleBuilderOptions<T, TProperty> ValidPageNumber<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, TProperty valueToCompare) where TProperty : 
            IComparable<TProperty>, IComparable 
        {
            return ruleBuilder.SetValidator(new GreaterThanOrEqualValidator<T,TProperty>(valueToCompare))
                .WithMessage("PageNumber at least greater than or equal to 1.");
        }
        
        public static IRuleBuilderOptions<T, TProperty> ValidPageSize<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder, TProperty valueToCompare) where TProperty : 
            IComparable<TProperty>, IComparable 
        {
            return ruleBuilder.SetValidator(new GreaterThanOrEqualValidator<T,TProperty>(valueToCompare))
                .WithMessage("PageSize at least greater than or equal to 1.");
        }
    }
}