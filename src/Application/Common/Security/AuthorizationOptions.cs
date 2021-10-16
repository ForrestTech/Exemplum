namespace Exemplum.Application.Common.Security
{
    public class AuthorizationOptions
    {
        public bool AuthorizationEnabled { get; set; } = true;

        public bool RequireAuthorizationByDefault { get; set; } = true;
    }
}