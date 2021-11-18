namespace Exemplum.Infrastructure.Persistence.ExceptionHandling;

public interface IHandlerSpecificDBException
{
    bool CanHandle(Exception exception);
    void HandleException(Exception exception);
}