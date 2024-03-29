namespace Exemplum.UnitTests.Application.Exceptions;

using Exemplum.Application.Common.Exceptions;
using Exemplum.Application.Common.Exceptions.Converters;

public class UnauthorizedAccessExceptionConverterTests
{
    [Fact]
    public void Convert_should_set_message()
    {
        var sut = new ExceptionToErrorConverter(new List<ICustomExceptionErrorConverter>
        {
            new UnauthorizedAccessExceptionConverter()
        });

        var errorInfo = sut.Convert(new UnauthorizedAccessException(), true);

        errorInfo.Message.Should().Be("Unauthorized");
    }
}