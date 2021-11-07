namespace Exemplum.Domain.Exceptions;

using System.Net;

public interface IHaveResponseCode
{
    HttpStatusCode StatusCode { get; }
}