using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace QandA.CrossCutting
{
	public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ILogger _logger;

		public LoggingBehavior(ILoggerFactory loggingFactory)
		{
			_logger = loggingFactory.CreateLogger("HandlerLoggingBehavior");
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
			RequestHandlerDelegate<TResponse> next)
		{
			var requestName = typeof(TRequest).Name;

			_logger.LogDebug("Handling request: '{requestName}' - {@request}", requestName, request);

			TResponse response;
			try
			{
				response = await next();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex,
					"Handler raised exception {exceptionName} during the handling of request: '{requestName}'",
					ex.GetType().Name, requestName);
				throw;
			}

			_logger.LogDebug("Finished handling request: '{requestName}' - Response: {@response}", requestName,
				response);

			return response;
		}
	}
}