namespace Exemplum.Application.Common.Behaviour
{
    using Identity;
    using MediatR.Pipeline;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using System.Threading.Tasks;

    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IUserIdentity _userIdentity;

        public LoggingBehaviour(ILogger<TRequest> logger, IUserIdentity userIdentity)
        {
            _logger = logger;
            _userIdentity = userIdentity;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _userIdentity.UserId ?? string.Empty;
            var userName = _userIdentity.GetUserNameAsync() ?? string.Empty;

            _logger.LogInformation("Request: {Name} {@UserId} {@UserName} {@Request}",
                requestName, userId, userName, request);
            
            return Task.CompletedTask;
        }
    }
}