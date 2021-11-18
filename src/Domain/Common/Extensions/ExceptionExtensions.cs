namespace Exemplum.Domain.Common.Extensions;

public static class ExceptionExtensions
{
    public static string GetAllMessages(this Exception exception)
    {
        var message = exception.Message;
        var messages = exception.GetInnerExceptions()
            .Select(x => x.Message)
            .ToList();

        if (messages.Any())
        {
            message += "InnerException: ";
            message += string.Join("", messages);
        }

        return message;
    }

    private static IEnumerable<Exception> GetInnerExceptions(this Exception exception)
    {
        var innerException = exception.InnerException;

        while (innerException != null)
        {
            yield return innerException;
            innerException = innerException.InnerException;
        }
    }
}