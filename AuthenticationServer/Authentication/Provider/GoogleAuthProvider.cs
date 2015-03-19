using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Google;

namespace Dijits.Authentication.Provider
{
    public class GoogleAuthProvider : IGoogleOAuth2AuthenticationProvider
    {
        public Task Authenticated(GoogleOAuth2AuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }

        public Task ReturnEndpoint(GoogleOAuth2ReturnEndpointContext context)
        {
            return Task.FromResult<object>(null);
        }

        public void ApplyRedirect(GoogleOAuth2ApplyRedirectContext context)
        {
            context.Response.Redirect(context.RedirectUri);
        }
    }
}