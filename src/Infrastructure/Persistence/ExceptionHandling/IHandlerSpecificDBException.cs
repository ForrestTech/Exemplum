namespace Infrastructure.Persistence.ExceptionHandling
{
    using System;

    public interface IHandlerSpecificDBException
    {
        bool CanHandle(Exception exception);
        void HandleException(Exception exception);
    }
}