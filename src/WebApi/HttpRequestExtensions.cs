namespace Exemplum.WebApi
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Net.Http.Headers;
    using System;

    public static class HttpRequestExtensions
    {
        public static bool IsAjax(this HttpRequest request)
        {
            return string.Equals(request.Query[HeaderNames.XRequestedWith], "XMLHttpRequest",
                       StringComparison.Ordinal) ||
                   string.Equals(request.Headers[HeaderNames.XRequestedWith], "XMLHttpRequest",
                       StringComparison.Ordinal);
        }

        public static bool CanAccept(this HttpRequest request, string contentType)
        {
            return request.Headers[HeaderNames.Accept].ToString()
                .Contains(contentType, StringComparison.OrdinalIgnoreCase);
        }
    }
}