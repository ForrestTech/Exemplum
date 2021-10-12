namespace Exemplum.Application.Common.Exceptions
{
    using Domain.Exceptions;
    using System;
    using System.Net;

    public class NotFoundException :
        Exception,
        IHaveResponseCode
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string name, object key)
            : base($"The resource does not exist. Resource Type '{name}' with key '{key}' was not found.")
        {
        }

        public HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;
    }
}