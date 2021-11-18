namespace Exemplum.Application.Common.Behaviour;

using Identity;
using MediatR.Pipeline;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;
    private readonly IUserIdentityService _userIdentityService;

    public LoggingBehaviour(ILogger<TRequest> logger, IUserIdentityService userIdentityService)
    {
        _logger = logger;
        _userIdentityService = userIdentityService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _userIdentityService.UserId ?? string.Empty;
        var userName = _userIdentityService.GetUserNameAsync() ?? string.Empty;

        _logger.LogInformation("Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);

        return Task.CompletedTask;
    }
}