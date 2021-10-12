namespace Exemplum.Domain.Exceptions
{
    using Microsoft.Extensions.Logging;

    public interface IExceptionWithSelfLogging
    {
        void Log(ILogger logger);
    }
}