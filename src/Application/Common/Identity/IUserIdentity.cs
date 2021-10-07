namespace Exemplum.Application.Common.Identity
{
    using System.Threading.Tasks;

    public interface IUserIdentity
    {
        string? UserId { get; }
        
        string? GetUserNameAsync();

        bool IsInRoleAsync(string role);
    }
}