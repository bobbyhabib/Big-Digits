using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Dijits.Authentication.Entities;
using Dijits.Authentication.Repository;
using Dijits.Authentication.Store;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Dijits.Authentication.Provider
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            string symmetricKeyAsBase64 = string.Empty;
            Client client = null;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id is not set");
                return Task.FromResult<object>(null);
            }

            //dynamic formCollection = (Microsoft.Owin.FormCollection)context.Parameters;
            //var audienceId = formCollection.Get("audience_id");

            // Find Audience ID From Collection.
            // Need to add to database.

            var audience = AudiencesStore.FindAudience(context.ClientId);

            if (audience == null)
            {
                context.SetError("invalid_clientId", string.Format("Invalid client_id '{0}'", context.ClientId));
                return Task.FromResult<object>(null);
            }

            ////using (AuthRepository _repo = new AuthRepository())
            ////{
            ////    client = _repo.FindClient(context.ClientId);
            ////}

            ////if (client == null)
            ////{
            ////    context.SetError("invalid_clientId", string.Format("Client '{0}' is not registered in the system.", context.ClientId));
            ////    return Task.FromResult<object>(null);
            ////}

            ////if (client.ApplicationType == ApplicationTypes.NativeConfidential)
            ////{
            ////    if (string.IsNullOrWhiteSpace(clientSecret))
            ////    {
            ////        context.SetError("invalid_clientId", "Client secret should be sent.");
            ////        return Task.FromResult<object>(null);
            ////    }
            ////    else
            ////    {
            ////        if (client.Secret != Helper.GetHash(clientSecret))
            ////        {
            ////            context.SetError("invalid_clientId", "Client secret is invalid.");
            ////            return Task.FromResult<object>(null);
            ////        }
            ////    }
            ////}

            ////if (!client.Active)
            ////{
            ////    context.SetError("invalid_clientId", "Client is inactive.");
            ////    return Task.FromResult<object>(null);
            ////}

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });



            // Dummy check here, you need to do your DB checks against membership system http://bit.ly/SPAAuthCode
            ////if (context.UserName != context.Password)
            ////{
            ////    context.SetError("invalid_grant", "The user name or password is incorrect");
            ////    return Task.FromResult<object>(null);
            ////}

            ////var identity = new ClaimsIdentity("JWT");

            ////identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            ////identity.AddClaim(new Claim("sub", context.UserName));
            ////identity.AddClaim(new Claim(ClaimTypes.Role, "Manager"));
            ////identity.AddClaim(new Claim(ClaimTypes.Role, "Supervisor"));

            using (AuthRepository _repo = new AuthRepository())
            {
                IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim("sub", context.UserName));

            //var props = new AuthenticationProperties(new Dictionary<string, string>
            //    {
            //        {
            //             "audience", (context.ClientId == null) ? string.Empty : context.ClientId
            //        }
            //    });

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                         "audience", (context.ClientId == null) ? string.Empty : context.ClientId
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
            // return Task.FromResult<object>(null);
        }
    }
}