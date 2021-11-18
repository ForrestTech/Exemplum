namespace Exemplum.Application.Common.Validation;

using Domain.Exceptions;
using Domain.Extensions;
using FluentValidation.Results;
using System.Net;
using System.Text;

public class ValidationException : Exception,
    IHaveLogLevel,
    IHaveResponseCode,
    IHaveValidationErrors,
    IExceptionWithSelfLogging
{
    public ValidationException(string propertyName, string errorMessage)
    {
        ValidationErrors = new Dictionary<string, string[]>();
        ValidationErrors.Add(propertyName, new[] {errorMessage});
    }

    public ValidationException(IDictionary<string, string[]> validationErrors)
    {
        ValidationErrors = validationErrors;
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this(string.Empty, failures)
    {
    }

    private ValidationException(string message, IEnumerable<ValidationFailure> failures)
        : base(message)
    {
        ValidationErrors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;

    public LogLevel LogLevel { get; set; } = LogLevel.Warning;

    public IDictionary<string, string[]> ValidationErrors { get; }

    public void Log(ILogger logger)
    {
        if (ValidationErrors.None())
        {
            return;
        }

        var validationErrors = new StringBuilder();
        validationErrors.AppendLine("There are " + ValidationErrors.Count + " validation errors:");
        foreach (var (propertyName, errors) in ValidationErrors)
        {
            var propertyErrors = $"({string.Join(", ", errors)})";

            validationErrors.AppendLine($"{propertyName} {propertyErrors}");
        }

        var logMessage = validationErrors.ToString();

        logger.Log(LogLevel, logMessage);
    }
}