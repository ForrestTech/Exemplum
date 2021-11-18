namespace Exemplum.Application.Common.Exceptions;

using Domain.Common.Extensions;
using Domain.Exceptions;
using Domain.Extensions;
using System.Net;

public class ExceptionToErrorConverter : IExceptionToErrorConverter
{
    private readonly IEnumerable<ICustomExceptionErrorConverter> _customConverters;

    public ExceptionToErrorConverter(IEnumerable<ICustomExceptionErrorConverter> customConverters)
    {
        _customConverters = customConverters;
    }

    public ErrorInfo Convert(Exception exception, bool includeSensitiveDetails)
    {
        var customConverter = _customConverters.SingleOrDefault(x => x.CanConvert(exception));

        if (customConverter != null)
        {
            var info = customConverter.Convert(exception, includeSensitiveDetails);
            return info;
        }

        var code = GetErrorCode(exception);
        var responseCode = GetResponseCode(exception);
        var detail = GetErrorDetails(exception, includeSensitiveDetails);
        var validationErrors = GetValidationErrors(exception);

        return new ErrorInfo
        {
            Code = code,
            ResponseCode = responseCode,
            Message = exception.Message,
            Details = detail,
            Data = exception.Data,
            ValidationErrors = validationErrors
        };
    }

    private static IDictionary<string, string[]> GetValidationErrors(Exception exception)
    {
        IDictionary<string, string[]> validationErrors = new Dictionary<string, string[]>();
        if (exception is IHaveValidationErrors haveValidationErrors)
        {
            validationErrors = haveValidationErrors.ValidationErrors;
        }

        return validationErrors;
    }

    private static string GetErrorDetails(Exception exception, bool includeSensitiveDetails)
    {
        var detail = string.Empty;
        if (exception is IHaveErrorDetails haveErrorDetails)
        {
            detail = haveErrorDetails.Details;
        }

        if (includeSensitiveDetails && detail.HasNoValue())
        {
            detail = exception.GetAllMessages();
        }

        return detail;
    }

    private static string GetErrorCode(Exception exception)
    {
        var code = string.Empty;
        if (exception is IHaveErrorCode haveErrorCode)
        {
            code = haveErrorCode.Code;
        }

        return code;
    }

    private static HttpStatusCode GetResponseCode(Exception exception)
    {
        var responseCode = HttpStatusCode.BadRequest;
        if (exception is IHaveResponseCode haveResponseCode)
        {
            responseCode = haveResponseCode.StatusCode;
        }

        return responseCode;
    }
}