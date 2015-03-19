using System;
using System.Web.Http;
using Dijits.Authentication;
using Dijits.Authentication.Formats;
using Dijits.Authentication.Provider;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
[assembly: OwinStartup(typeof(StartUp))]
namespace Dijits.Authentication
{
    public class StartUp
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

        /// <summary>
        /// The configuration.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            this.ConfigureOAuth(app);
            // WebApiConfig.Register(config: config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(configuration: config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            //use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ExternalCookie);
            // OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            ////var oAuthServerOptions = new OAuthAuthorizationServerOptions
            ////{
            ////    AllowInsecureHttp = true,
            ////    TokenEndpointPath = new PathString("/token"),
            ////    AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
            ////    Provider = new SimpleAuthorizationServiceProvider(),
            ////    RefreshTokenProvider = new SimpleRefreshTokenProvider()

            ////};

            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev environment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth2/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat("http://localhost:37304/")
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            //// app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            //// app.UseOAuthBearerAuthentication(OAuthBearerOptions);


            //Configure Google External Login
            googleAuthOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "499523076962-fikg8th60rvvvlsngfpkk6fccsb2v28g.apps.googleusercontent.com",
                ClientSecret = "pTQuMNphvxXmiJrldv1DzhV-",
                Provider = new GoogleAuthProvider()
            };
            app.UseGoogleAuthentication(googleAuthOptions);

            //Configure Facebook External Login
            facebookAuthOptions = new FacebookAuthenticationOptions()
            {
                AppId = "853901721335004",
                AppSecret = "a4c222761a2a4c1768c1fb554d48b207",
                Provider = new FacebookAuthProvider()
            };
            app.UseFacebookAuthentication(facebookAuthOptions);
        }
    }
}