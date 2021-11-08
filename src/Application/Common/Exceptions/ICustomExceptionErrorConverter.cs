namespace Exemplum.Application.Common.Exceptions;

public interface ICustomExceptionErrorConverter
{
    bool CanConvert(Exception exception);

    ErrorInfo Convert(Exception exception, bool includeSensitiveDetails);
}