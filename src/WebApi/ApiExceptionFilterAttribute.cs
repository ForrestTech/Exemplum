namespace Exemplum.WebApi
{
    using Application.Common.Exceptions;
    using Domain.Extensions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Hosting;
    using System.Collections;
    using System.Linq;
    using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IExceptionToErrorConverter _converter;
        private readonly IWebHostEnvironment _env;

        public ApiExceptionFilterAttribute(IExceptionToErrorConverter converter, IWebHostEnvironment env)
        {
            _converter = converter;
            _env = env;
        }

        public override void OnException(ExceptionContext context)
        {
            if (!ShouldHandleException(context))
            {
                return;
            }

            HandleException(context);

            base.OnException(context);
        }

        private static bool ShouldHandleException(ActionContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                return true;
            }

            return context.HttpContext.Request.CanAccept(MimeTypes.Application.Json) ||
                   context.HttpContext.Request.IsAjax();
        }

        private void HandleException(ExceptionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            var errorInfo = _converter.Convert(context.Exception, _env.IsDevelopment());

            if (errorInfo.ValidationErrors.Any())
            {
                var validationProblemDetails = new ValidationProblemDetails(errorInfo.ValidationErrors);

                context.Result = new BadRequestObjectResult(validationProblemDetails);

                context.ExceptionHandled = true;

                return;
            }

            var details = new ProblemDetails { Title = errorInfo.Message, Detail = errorInfo.Details };

            if (errorInfo.Code.HasValue())
            {
                details.Extensions.Add("code", errorInfo.Code);
            }

            foreach (DictionaryEntry data in errorInfo.Data)
            {
                details.Extensions.Add(data.Key.ToString()?.ToLowerInvariant() ?? string.Empty, data.Value);
            }

            context.Result = new ObjectResult(details) { StatusCode = (int)errorInfo.ResponseCode };

            context.ExceptionHandled = true;
        }

        private static void HandleInvalidModelStateException(ExceptionContext context)
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }
    }
}