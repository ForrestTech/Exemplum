namespace Exemplum.Application.Common.Exceptions
{
    using System;

    public interface ICustomExceptionErrorConverter
    {
        bool CanConvert(Exception exception);

        ErrorInfo Convert(Exception exception, bool includeSensitiveDetails);
    }
}