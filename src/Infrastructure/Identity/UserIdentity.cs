namespace Exemplum.Infrastructure.Identity
{
    using Application.Common.Identity;
    using System.Linq;
    using System.Security.Claims;

    public class UserIdentity : IUserIdentity
    {
        private readonly ICurrentUser _currentUser;

        public UserIdentity(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public string? UserId
        {
            get
            {
                return _currentUser.UserId;
            }
        }

        public string? GetUserNameAsync()
        {
            var user = _currentUser.UserId;

            return user;
        }
        
        public bool IsInRoleAsync(string role)
        {
            if (_currentUser.Principal?.Claims == null)
            {
                return false;
            }

            var roles = _currentUser.Principal?.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value);

            return roles != null && roles.Contains(role);
        }
    }
}