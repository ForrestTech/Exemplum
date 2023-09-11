namespace Exemplum.Application.Common.Security;

using Domain.Extensions;
using Identity;
using MediatR;
using Microsoft.Extensions.Options;
using System.Reflection;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUserIdentityService _identityServiceService;
    private readonly IExemplumAuthorizationService _authorizationService;
    private readonly AuthorizationOptions _authorizationOptions;

    public AuthorizationBehaviour(IOptions<AuthorizationOptions> options,
        IUserIdentityService identityServiceService,
        IExemplumAuthorizationService authorizationService)
    {
        _identityServiceService = identityServiceService;
        _authorizationService = authorizationService;
        _authorizationOptions = options.Value;
    }


    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_authorizationOptions.AuthorizationEnabled == false)
        {
            return await next();
        }

        var allowAnonymousAccessAttributes = request.GetType()
            .GetCustomAttributes<AllowAnonymousAccessAttribute>()
            .ToList();

        if (allowAnonymousAccessAttributes.Any())
        {
            return await next();
        }

        var authorizeAttributes = request.GetType()
            .GetCustomAttributes<AuthorizeAttribute>()
            .ToList();

        var requestRequiresAuthenticatedUser =
            _authorizationOptions.RequireAuthorizationByDefault || authorizeAttributes.Any();
        if (requestRequiresAuthenticatedUser)
        {
            var unAuthenticatedUser = _identityServiceService.UserId == null;
            if (unAuthenticatedUser)
            {
                throw new UnauthorizedAccessException();
            }
        }

        var requestHasNoAuthorizationRequirements = !authorizeAttributes.Any(x => x.HasAuthenticationRequirements);
        if (requestHasNoAuthorizationRequirements)
        {
            return await next();
        }

        HandleRoleAuthorization(authorizeAttributes);

        await HandlePolicyAuthorization(authorizeAttributes);

        return await next();
    }

    private void HandleRoleAuthorization(IEnumerable<AuthorizeAttribute> authorizeAttributes)
    {
        var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles))
            .ToList();

        if (authorizeAttributesWithRoles.None())
        {
            return;
        }

        var authorized = false;

        foreach (var roles in authorizeAttributesWithRoles.Select(a => a.Roles.Split(',')))
        {
            if (roles.Select(role => _identityServiceService.IsInRoleAsync(role.Trim())).Any(isInRole => isInRole))
            {
                authorized = true;
            }
        }

        if (!authorized)
        {
            throw new ForbiddenAccessException(typeof(TRequest))
            {
                Roles = authorizeAttributesWithRoles.Select(x => x.Roles)
            };
        }
    }

    private async Task HandlePolicyAuthorization(IEnumerable<AuthorizeAttribute> authorizeAttributes)
    {
        var authorizeAttributesWithPolicies = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy))
            .ToList();

        if (authorizeAttributesWithPolicies.None())
        {
            return;
        }

        foreach (var policy in authorizeAttributesWithPolicies.Select(a => a.Policy))
        {
            var authorized = await _authorizationService.AuthorizeAsync(policy);

            if (!authorized)
            {
                throw new ForbiddenAccessException(typeof(TRequest)) {Policy = policy};
            }
        }
    }
}