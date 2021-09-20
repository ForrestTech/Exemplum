namespace Exemplum.Infrastructure.Persistence.ExceptionHandling
{
    using System;

    public class NoOpHandleDbExceptions : IHandleDbExceptions
    {
        public void HandleException(Exception exception)
        { }
    }
}