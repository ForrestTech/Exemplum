namespace Exemplum.Application.Common.Exceptions.Converters;

using System.Net;

public class UnauthorizedAccessExceptionConverter : ICustomExceptionErrorConverter
{
    public bool CanConvert(Exception exception)
    {
        return exception is UnauthorizedAccessException;
    }

    public ErrorInfo Convert(Exception exception, bool includeSensitiveDetails)
    {
        if (exception is not UnauthorizedAccessException unauthorizedException)
        {
            return new ErrorInfo();
        }

        var info = new ErrorInfo {Message = "Unauthorized", ResponseCode = HttpStatusCode.Unauthorized};

        return info;
    }
}