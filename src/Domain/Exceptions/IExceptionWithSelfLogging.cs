namespace Exemplum.Domain.Exceptions;

public interface IExceptionWithSelfLogging
{
    void Log(ILogger logger);
}