namespace Exemplum.WebApi;

using Application.Common.Exceptions;
using Domain.Extensions;
using Microsoft.AspNetCore.Diagnostics;

public static class ApiExceptionMiddlewareExtensions
{
    private static readonly MediaTypeHeaderValue ProblemJsonMediaType = new("application/problem+json");

    public static void UseApiExceptionHandler(this WebApplication app, bool includeDetails)
    {
        app.UseExceptionHandler("/error");

        app.Map("/error", IResult (HttpContext context) =>
        {
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            var converter = context.RequestServices.GetService<IExceptionToErrorConverter>();

            if (exception == null || converter == null)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }

            var errorInfo = converter.Convert(exception, includeDetails);

            var details = new ProblemDetails()
            {
                Title = errorInfo.Message, Detail = errorInfo.Details, Status = (int?)errorInfo.ResponseCode
            };

            if (errorInfo.Code.HasValue())
            {
                details.Extensions.Add("code", errorInfo.Code);
            }

            foreach (DictionaryEntry data in errorInfo.Data)
            {
                details.Extensions.Add(data.Key.ToString()?.ToLowerInvariant() ?? string.Empty, data.Value);
            }


            details.Extensions.Add("requestId", Activity.Current?.Id ?? context.TraceIdentifier);

            if (context.Request.GetTypedHeaders().Accept?.Any(h => ProblemJsonMediaType.IsSubsetOf(h)) == true)
            {
                var extensions = new Dictionary<string, object?>
                {
                    { "requestId", Activity.Current?.Id ?? context.TraceIdentifier }
                };

                return exception switch
                {
                    BadHttpRequestException ex => Results.Problem(ex.Message, statusCode: ex.StatusCode,
                        extensions: extensions),
                    _ => Results.Problem(details)
                };
            }

            return Results.StatusCode(details.Status ?? StatusCodes.Status500InternalServerError);
        }).ExcludeFromDescription();
    }
}