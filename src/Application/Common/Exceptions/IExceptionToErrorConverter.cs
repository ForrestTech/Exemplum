namespace Exemplum.Application.Common.Exceptions;

public interface IExceptionToErrorConverter
{
    ErrorInfo Convert(Exception exception, bool includeSensitiveDetails);
}