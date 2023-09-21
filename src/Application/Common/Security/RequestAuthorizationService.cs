namespace Exemplum.Application.Common.Security;

using Identity;
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

    public async Task<(bool Allowed, string DeniedReason)> AuthorizeRequestAsync
        <TRequest>(
            TRequest request,
            object resource,
            string policyName)
    {
        var user = _currentUser.Principal;

        if (user == null)
        {
            return Denied(request, policyName);
        }

        var result = await _authorizationService.AuthorizeAsync(user, resource, policyName);

        if (result.Succeeded)
        {
            return (true, string.Empty);
        }

        if (result.Failure != null && result.Failure.FailureReasons.Any())
        {
            return Denied(request, result.Failure.FailureReasons.First().Message);
        }

        return Denied(request, policyName);
    }

    private static (bool Allowed, string DeniedReason) Denied<TRequest>(TRequest request, string policyName)
    {
        return (false, $"Access denied to request {request?.GetType().Name} with policy {policyName}");
    }
}