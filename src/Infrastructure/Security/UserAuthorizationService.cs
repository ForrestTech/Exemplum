namespace Exemplum.Infrastructure.Security;

using Application.Common.Identity;
using Application.Common.Security;
using Microsoft.AspNetCore.Authorization;

public class UserAuthorizationService : IUserAuthorizationService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly ICurrentUser _currentUser;

    public UserAuthorizationService(
        IAuthorizationService authorizationService,
        ICurrentUser currentUser)
    {
        _authorizationService = authorizationService;
        _currentUser = currentUser;
    }

    public async Task<bool> AuthorizeAsync(string policyToCheck)
    {
        if (_currentUser.Principal == null)
        {
            return false;
        }

        var result = await _authorizationService.AuthorizeAsync(_currentUser.Principal, policyToCheck);

        return result.Succeeded;
    }
}