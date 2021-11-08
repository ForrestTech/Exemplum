namespace Exemplum.Infrastructure.Identity;

using Application.Common.Identity;
using Application.Common.Security;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

/// <summary>
/// A fake user that can be used during dev to quickly change roles nad permissions for testing
/// </summary>
public class FakeUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FakeUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ??
                             "richard.a.forrest@gmail.com";

    public ClaimsPrincipal? Principal =>
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) != null
            ? _httpContextAccessor.HttpContext?.User
            : new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, UserId!),
                new(ClaimTypes.Role, Security.Roles.Forecaster),
                new(Security.ClaimTypes.Permission, Security.Permissions.WriteTodo),
                new(Security.ClaimTypes.Permission, Security.Permissions.DeleteTodo)
            }));
}