namespace Exemplum.Application.Common.Exceptions;

using System.Collections;
using System.Net;

public class ErrorInfo
{
    public string Code { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Details { get; set; } = string.Empty;

    public HttpStatusCode ResponseCode { get; set; } = HttpStatusCode.InternalServerError;

    public IDictionary Data { get; set; } = new Dictionary<string, string>();
}