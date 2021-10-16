namespace Exemplum.Application.Common.Identity
{
    using System.Threading.Tasks;

    public interface IExemplumAuthorizationService
    {
        Task<bool> AuthorizeAsync(string policyName);
    }
}