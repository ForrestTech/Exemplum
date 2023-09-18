namespace Exemplum.Application.Common.Validation;

using FluentValidation.Results;

public class ValidationFailed
{
    private readonly List<ValidationFailure> _failures;

    public ValidationFailed(List<ValidationFailure> failures)
    {
        _failures = failures;
    }
    
    public ValidationFailed(string propertyName, string errorMessage)
    {
        _failures = new List<ValidationFailure> {new(propertyName, errorMessage)};
    }

    public Dictionary<string, string[]> Errors
    {
        get { return _failures.GroupBy(f => f.PropertyName, f => f.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray()); }
    }
}