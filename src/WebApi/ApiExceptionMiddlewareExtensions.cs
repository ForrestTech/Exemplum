namespace Exemplum.WebApi;

using Application.Common.Exceptions;
using Domain.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

public static class ApiExceptionMiddlewareExtensions
{
    private static readonly MediaTypeHeaderValue ProblemJsonMediaType = new("application/problem+json");

    public static void UseApiExceptionHandler(this WebApplication app, bool includeDetails)
    {
        app.UseExceptionHandler("/error");

        app.Map("/error", Results<Problem, StatusCode>(HttpContext context) =>
        {
            var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            var converter = context.RequestServices.GetService<IExceptionToErrorConverter>();

            if (error == null || converter == null)
            {
                return Results.Extensions.StatusCode(StatusCodes.Status500InternalServerError,
                    "An unhandled exception occurred while processing the request.");
            }

            var errorInfo = converter.Convert(error, includeDetails);

            ProblemDetails details;

            if (errorInfo.ValidationErrors.Any())
            {
                details = new ValidationProblemDetails(errorInfo.ValidationErrors);
                details.Status = (int?)HttpStatusCode.BadRequest;
            }
            else
            {
                details = new ProblemDetails {Title = errorInfo.Message, Detail = errorInfo.Details, Status = (int?)errorInfo.ResponseCode};

                if (errorInfo.Code.HasValue())
                {
                    details.Extensions.Add("code", errorInfo.Code);
                }

                foreach (DictionaryEntry data in errorInfo.Data)
                {
                    details.Extensions.Add(data.Key.ToString()?.ToLowerInvariant() ?? string.Empty, data.Value);
                }
            }

            details.Extensions.Add("requestId", Activity.Current?.Id ?? context.TraceIdentifier);

            if (context.Request.GetTypedHeaders().Accept?.Any(h => ProblemJsonMediaType.IsSubsetOf(h)) == true)
            {
                var extensions = new Dictionary<string, object> {{"requestId", Activity.Current?.Id ?? context.TraceIdentifier}};

                return error switch
                {
                    BadHttpRequestException ex => Results.Extensions.Problem(ex.Message, statusCode: ex.StatusCode,
                        extensions: extensions),
                    _ => Results.Extensions.Problem(details)
                };
            }

            return Results.Extensions.StatusCode(details.Status ?? StatusCodes.Status500InternalServerError,
                details.Title);
        }).ExcludeFromDescription();
    }
}