namespace Exemplum.Domain.Exceptions
{
    using System.Collections.Generic;

    public interface IHaveValidationErrors
    {
        IDictionary<string, string[]> ValidationErrors { get; }
    }
}