namespace Exemplum.Application.Common.Identity;

public interface IExemplumAuthorizationService
{
    Task<bool> AuthorizeAsync(string policyName);
}