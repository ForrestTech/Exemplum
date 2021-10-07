namespace Exemplum.Application.Common.Identity
{
    using System.Threading.Tasks;

    public interface IAuthenticationService
    {
        Task<bool> AuthorizeAsync(string policyName);
    }
}