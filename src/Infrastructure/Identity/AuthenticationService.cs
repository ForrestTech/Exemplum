namespace Exemplum.Infrastructure.Identity
{
    using Application.Common.Identity;
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;

    public class AuthenticationService : IAuthenticationService
    {
        
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUser _currentUser;

        public AuthenticationService(
            IAuthorizationService authorizationService,
            ICurrentUser currentUser)
        {
            _authorizationService = authorizationService;
            _currentUser = currentUser;
        }
        
        public async Task<bool> AuthorizeAsync(string policyName)
        {
            if (_currentUser.Principal == null)
            {
                return false;
            }

            var result = await _authorizationService.AuthorizeAsync(_currentUser.Principal, policyName);

            return result.Succeeded;
        }
    }
}