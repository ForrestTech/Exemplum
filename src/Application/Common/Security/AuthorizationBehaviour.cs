namespace Exemplum.Application.Common.Security
{
    using Domain.Extensions;
    using Identity;
    using MediatR;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IUserIdentity _identityService;
        private readonly IAuthenticationService _authenticationService;
        private readonly AuthorizationOptions _authorizationOptions;

        public AuthorizationBehaviour(IOptions<AuthorizationOptions> options,
            IUserIdentity identityService,
            IAuthenticationService authenticationService)
        {
            _identityService = identityService;
            _authenticationService = authenticationService;
            _authorizationOptions = options.Value;
        }


        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var authorizeAttributes = request.GetType()
                .GetCustomAttributes<AuthorizeAttribute>()
                .ToList();

            if (authorizeAttributes.None())
            {
                return await next();
            }

            if (_authorizationOptions.AuthorizationEnabled == false)
            {
                return await next();
            }

            var unAuthenticatedUser = _identityService.UserId == null;
            if (unAuthenticatedUser)
            {
                throw new UnauthorizedAccessException();
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
                if (roles.Select(role => _identityService.IsInRoleAsync(role.Trim())).Any(isInRole => isInRole))
                {
                    authorized = true;
                }
            }

            if (!authorized)
            {
                throw new ForbiddenAccessException();
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
                var authorized = await _authenticationService.AuthorizeAsync(policy);

                if (!authorized)
                {
                    throw new ForbiddenAccessException();
                }
            }
        }
    }

    public class AuthorizationOptions
    {
        public bool AuthorizationEnabled { get; set; } 
    }
}