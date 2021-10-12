namespace Exemplum.Application.Common.Exceptions
{
    using System;

    public interface IExceptionToErrorConverter
    {
        ErrorInfo Convert(Exception exception, bool includeSensitiveDetails);
    }
}