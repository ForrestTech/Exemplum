namespace Exemplum.Application.Common.Security;

public interface IRequestAuthorizationService
{
    Task<(bool Allowed, string DeniedReason)> AuthorizeRequestAsync<TRequest>(TRequest request, object resource, string policyName);
}
