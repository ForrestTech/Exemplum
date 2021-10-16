namespace Exemplum.Application.Common.Identity
{
    public interface IUserIdentityService
    {
        string? UserId { get; }
        
        string? GetUserNameAsync();

        bool IsInRoleAsync(string role);
    }
}