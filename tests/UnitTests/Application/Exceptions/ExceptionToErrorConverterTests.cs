namespace Exemplum.IntegrationTests.Application.Exceptions
{
    using AutoFixture;
    using Exemplum.Application.Common.Exceptions;
    using Exemplum.Application.Common.Validation;
    using Exemplum.Domain.Exceptions;
    using FluentAssertions;
    using System.Collections.Generic;
    using Xunit;

    public class ExceptionToErrorConverterTests
    {
        [Fact]
        public void Convert_maps_error_details_to_info()
        {
            var fixture = new Fixture();

            var sut = new ExceptionToErrorConverter(new List<ICustomExceptionErrorConverter>());

            var exception = fixture.Create<BusinessException>();

            var errorInfo = sut.Convert(exception, true);

            errorInfo.Code.Should().Be(exception.Code);
            errorInfo.Message.Should().Be(exception.Message);
            errorInfo.Details.Should().Be(exception.Details);
            errorInfo.Data.Should().Be(exception.Data);
        }

        [Fact]
        public void Convert_maps_data_from_exceptions()
        {
            var fixture = new Fixture();

            var sut = new ExceptionToErrorConverter(new List<ICustomExceptionErrorConverter>());

            var exception = fixture.Create<BusinessException>();

            exception.WithData("foo", "bar");

            var errorInfo = sut.Convert(exception, true);

            errorInfo.Data.Should().Be(exception.Data);
        }

        [Fact]
        public void Convert_validation_exception_should_map_errors()
        {
            var fixture = new Fixture();

            var sut = new ExceptionToErrorConverter(new List<ICustomExceptionErrorConverter>());

            var exception = fixture.Create<ValidationException>();

            var errorInfo = sut.Convert(exception, true);

            errorInfo.Message.Should().Be(exception.Message);
            errorInfo.ValidationErrors.Should().BeEquivalentTo(exception.ValidationErrors);
        }
    }
}