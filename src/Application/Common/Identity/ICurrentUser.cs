namespace Exemplum.Application.Common.Identity
{
    using System.Security.Claims;

    public interface ICurrentUser
    {
        string? UserId { get; }
        
        public ClaimsPrincipal? Principal { get; }
    }
}