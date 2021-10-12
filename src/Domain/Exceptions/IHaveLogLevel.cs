namespace Exemplum.Domain.Exceptions
{
    using Microsoft.Extensions.Logging;

    public interface IHaveLogLevel
    {
        LogLevel LogLevel { get; set; }
    }
}