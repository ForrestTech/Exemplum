namespace Exemplum.Domain.Exceptions;

public interface IHaveValidationErrors
{
    IDictionary<string, string[]> ValidationErrors { get; }
}