namespace Exemplum.Application.Common.Security;

using Exemplum.Domain.Common;
using Microsoft.AspNetCore.Authorization;

public class OwnsResourceHandler : AuthorizationHandler<OwnsResourceRequirement, BaseEntity>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        OwnsResourceRequirement requirement,
        BaseEntity resource)
    {
        if (context.User.Identity?.Name == resource.CreatedBy)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail(new AuthorizationFailureReason(this, "User does not own resource"));
        }
        return Task.CompletedTask;
    }
}