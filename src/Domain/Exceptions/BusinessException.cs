namespace Exemplum.Domain.Exceptions
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;

    /// <summary>
    /// This will be the most common base exception for any violation of business logic in the application
    /// </summary>
    public class BusinessException : Exception,
        IBusinessException,
        IHaveErrorCode,
        IHaveErrorDetails,
        IHaveResponseCode,
        IHaveLogLevel
    {
        public string Code { get; }

        public HttpStatusCode StatusCode { get; }

        public string Details { get; }

        public LogLevel LogLevel { get; set; }

        public BusinessException(
            string code = "",
            string message = "",
            string details = "",
            HttpStatusCode statusCode = HttpStatusCode.Forbidden,
            Exception innerException = null!,
            LogLevel logLevel = LogLevel.Warning)
            : base(message, innerException)
        {
            Code = code;
            Details = details;
            LogLevel = logLevel;
            StatusCode = statusCode;
        }

        public BusinessException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}