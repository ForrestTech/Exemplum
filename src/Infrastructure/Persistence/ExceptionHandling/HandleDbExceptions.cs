namespace Exemplum.Infrastructure.Persistence.ExceptionHandling;

/// <summary>
/// Database exceptions can be cryptic and its not ideal to show them to the end user.  We map given exceptions to a more users friendly DatabaseException that can be shown to an end user
/// </summary>
public class HandleDbExceptions : IHandleDbExceptions
{
    private readonly IEnumerable<IHandlerSpecificDBException> _handlers;

    public HandleDbExceptions(IEnumerable<IHandlerSpecificDBException> handlers)
    {
        _handlers = handlers;
    }
    
    public void HandleException(Exception exception)
    {
        var handler = _handlers.SingleOrDefault(x => x.CanHandle(exception));

        handler?.HandleException(exception);
    }
}