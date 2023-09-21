namespace Exemplum.Application.Common.Security;

public interface IUserAuthorizationService
{
    Task<bool> AuthorizeAsync(string policyToCheck);
}