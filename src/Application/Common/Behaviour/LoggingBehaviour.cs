namespace Exemplum.Application.Common.Behaviour
{
    using MediatR.Pipeline;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger<LoggingBehaviour<TRequest>> _logger;
        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest>> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogInformation("Handling Request: {Name} {@Request}",
                requestName, request);

            return Task.CompletedTask;
        }
    }
}