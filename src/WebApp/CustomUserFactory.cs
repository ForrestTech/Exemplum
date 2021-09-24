namespace Exemplum.WebApp
{
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
    using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
    using System.Diagnostics;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// Update the user identity claims with the roles and permissions that are added to the access token
    /// </summary>
    public class CustomUserFactory
        : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        public CustomUserFactory(IAccessTokenProviderAccessor accessor)
            : base(accessor)
        {
        }

        public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
            RemoteUserAccount account,
            RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);

            Debug.Assert(user.Identity != null, "user.Identity != null");
            
            if (user.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)user.Identity;

                var accessTokenResult = await TokenProvider.RequestAccessToken();

                if (!accessTokenResult.TryGetToken(out var accessToken))
                {
                    return user;
                }

                // having to parse the token here as the provider does not give you the raw claims
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(accessToken.Value);

                if (jsonToken is not JwtSecurityToken token)
                {
                    return user;
                }

                foreach (var claim in token.Claims.Where(x => x.Type is "permissions" or ClaimTypes.Role))
                {
                    identity.AddClaim(new Claim(claim.Type, claim.Value));
                }
            }

            return user;
        }
    }
}