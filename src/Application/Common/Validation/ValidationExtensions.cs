namespace Exemplum.Application.Common.Validation;

using FluentValidation.Results;

public static class ValidationExtensions
{
    public static ValidationFailed ToFailure(this ValidationResult result)
    {
        return new ValidationFailed(result.Errors);
    }

    public static bool IsInvalid(this ValidationResult result)
    {
        return !result.IsValid;
    }
}