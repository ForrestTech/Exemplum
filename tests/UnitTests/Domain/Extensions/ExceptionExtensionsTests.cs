namespace Exemplum.UnitTests.Domain.Extensions;

using Exemplum.Domain.Common.Extensions;

public class ExceptionExtensionsTests
{
    [Fact]
    public void GetAllMessages_should_contain_all_exception_messages()
    {
        const string firstExceptionMessage = "First exception";
        const string innerExceptionMessage = "another exception";

        var sut = new Exception(firstExceptionMessage, new Exception(innerExceptionMessage));

        var allMessages = sut.GetAllMessages();

        allMessages.Should().Contain(firstExceptionMessage);
        allMessages.Should().Contain(innerExceptionMessage);
        allMessages.Should().Contain("InnerException:");
    }

    [Fact]
    public void GetAllMessages_should_not_contain_inner_exceptions_when_none_exist()
    {
        const string firstExceptionMessage = "First exception";

        var sut = new Exception(firstExceptionMessage);

        var allMessages = sut.GetAllMessages();

        allMessages.Should().Contain(firstExceptionMessage);
        allMessages.Should().NotContain("InnerException:");
    }
}