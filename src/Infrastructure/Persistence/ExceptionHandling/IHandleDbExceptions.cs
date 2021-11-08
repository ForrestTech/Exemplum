namespace Exemplum.Infrastructure.Persistence.ExceptionHandling;

using System;

public interface IHandleDbExceptions
{
    void HandleException(Exception exception);
}