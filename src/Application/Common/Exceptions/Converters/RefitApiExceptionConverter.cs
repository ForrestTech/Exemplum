namespace Exemplum.Application.Common.Exceptions.Converters
{
    using Refit;
    using System;

    public class RefitApiExceptionConverter : ICustomExceptionErrorConverter
    {
        public bool CanConvert(Exception exception)
        {
            return exception is ApiException;
        }

        public ErrorInfo Convert(Exception exception, bool includeSensitiveDetails)
        {
            if (exception is not ApiException apiException)
            {
                return new ErrorInfo();
            }

            var info = new ErrorInfo { Message = "An error occurred while processing your request." };

            if (includeSensitiveDetails)
            {
                info.Details = $"Exception calling external system.  Uri:'{apiException?.Uri}' " +
                               $"StatusCode: '{apiException?.StatusCode}' Reason:'{apiException?.ReasonPhrase}' " +
                               $"Message:'{exception?.Message}'";
            }

            return info;
        }
    }
}