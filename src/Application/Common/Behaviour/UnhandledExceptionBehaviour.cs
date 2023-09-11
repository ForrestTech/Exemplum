namespace Exemplum.Application.Common.Behaviour;

using Domain.Exceptions;

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger;

    public UnhandledExceptionBehaviour(ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            LogLevel logLevel = LogLevel.Error;
            if (ex is IHaveLogLevel hasLogLevel)
            {
                logLevel = hasLogLevel.LogLevel;
            }

            var requestName = typeof(TRequest).Name;
            _logger.Log(logLevel, ex, "Request: Unhandled '{Exception}' for Request {Name} {@Request}",
                ex.GetType().Name, requestName,
                request);

            if (ex is IExceptionWithSelfLogging withSelfLogging)
            {
                withSelfLogging.Log(_logger);
            }

            throw;
        }
    }
}