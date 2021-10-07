namespace Exemplum.Application.Common.Identity
{
    using System.Threading.Tasks;

    public interface IIdentity
    {
        Task<string> GetUserNameAsync(string userId);
    }

    public interface IAuthenticationService
    {
        Task<bool> AuthorizeAsync(string userId, string policyName);
    }
}