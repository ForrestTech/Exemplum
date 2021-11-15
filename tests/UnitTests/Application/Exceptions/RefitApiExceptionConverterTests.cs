namespace Exemplum.UnitTests.Application.Exceptions;

using Exemplum.Application.Common.Exceptions;
using Exemplum.Application.Common.Exceptions.Converters;
using Refit;

public class RefitApiExceptionConverterTests
{
    [Fact]
    public async Task Refit_converter_should_be_used_if_exception_type_match()
    {
        var sut = new ExceptionToErrorConverter(new List<ICustomExceptionErrorConverter>
        {
            new RefitApiExceptionConverter()
        });

        var exception = await ApiException.Create(
            new HttpRequestMessage(HttpMethod.Get, new Uri("https://something.com")),
            HttpMethod.Get,
            new HttpResponseMessage(HttpStatusCode.NotFound),
            new RefitSettings());

        var errorInfo = sut.Convert(exception, true);

        errorInfo.Message.Should().Be("An error occurred while processing your request.");
    }

    [Fact]
    public async Task Convert_add_api_request_details_if_include_sensitive_data_is_set()
    {
        var sut = new ExceptionToErrorConverter(new List<ICustomExceptionErrorConverter>
        {
            new RefitApiExceptionConverter()
        });

        var httpMethod = HttpMethod.Get;
        const string uri = "https://something.com/";
        const HttpStatusCode responseCode = HttpStatusCode.NotFound;

        var exception = await ApiException.Create(new HttpRequestMessage(httpMethod, new Uri(uri)),
            httpMethod,
            new HttpResponseMessage(responseCode),
            new RefitSettings());

        var errorInfo = sut.Convert(exception, true);

        errorInfo.Message.Should().Be("An error occurred while processing your request.");
        errorInfo.Details.Should().Contain(uri);
        errorInfo.Details.Should().Contain(responseCode.ToString());
    }
}