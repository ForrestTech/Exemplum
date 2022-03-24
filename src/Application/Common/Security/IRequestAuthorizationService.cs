namespace Exemplum.Application.Common.Security;

public interface IRequestAuthorizationService
{
    Task AuthorizeRequestAsync<TRequest>(object resource, string policyName);
}
