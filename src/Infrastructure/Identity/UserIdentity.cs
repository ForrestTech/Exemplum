namespace Exemplum.Infrastructure.Identity
{
    using Application.Common.Identity;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserIdentity : IUserIdentity
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUser _currentUser;

        public UserIdentity(UserManager<ApplicationUser> userManager, 
            ICurrentUser currentUser)
        {
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public string? UserId
        {
            get
            {
                return _currentUser.UserId;
            }
        }

        public async Task<string> GetUserNameAsync()
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == _currentUser.UserId);

            return user.UserName;
        }
        
        public async Task<bool> IsInRoleAsync(string role)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == _currentUser.UserId);

            if (user == null)
            {
                return false;
            }

            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}