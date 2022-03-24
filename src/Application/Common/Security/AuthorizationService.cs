namespace Exemplum.Application.Common.Security;

using Exemplum.Application.Common.Identity;
using Microsoft.AspNetCore.Authorization;

public class RequestAuthorizationService : IRequestAuthorizationService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICurrentUser _currentUser;

    public RequestAuthorizationService(IAuthorizationService authorizationService,
        ICurrentUser currentUser)
    {
        _authorizationService = authorizationService;
        _currentUser = currentUser;
    }
    public async Task AuthorizeRequestAsync<TRequest>(object resource, string policyName)
    {
        var user = _currentUser.Principal;

        if (user == null)
        {
            throw new ForbiddenAccessException(typeof(TRequest)) { Policy = policyName };
        }

        var result = await _authorizationService.AuthorizeAsync(user, resource, policyName);

        if (!result.Succeeded)
        {
            if (result.Failure != null && result.Failure.FailureReasons.Any())
            {
                throw new ForbiddenAccessException(typeof(TRequest))
                {
                    ForbiddenReason = string.Join("\r\n", result.Failure.FailureReasons.Select(x => x.Message))
                };
            }
            else
            {
                throw new ForbiddenAccessException(typeof(TRequest)) { Policy = policyName };
            }
        }
    }
}
