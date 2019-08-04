using DACServices.Api.Helpers.OAuth2;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DACServices.Api
{
    public partial class Startup
    {
        /// <summary>
        /// OAUTH options property
        /// </summary>
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        /// <summary>
        /// Public client id porperty
        /// </summary>
        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864 
        public void ConfigureAuth(IAppBuilder app)
        {
            //Enable the application to use a cookie  to store information for the signed in user
            //and to use a cookie to temporarily store information about a user loggin in with a third party login provider
            //Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/LogOff"),
                ExpireTimeSpan = TimeSpan.FromMinutes(5.0)
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new DACOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(4),
                AllowInsecureHttp = true //Do not do this in production ONLY FOR DEVELOPING: ALLOW INSECURE HTTP!!
            };

            //Enabled application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            //Enables the application  to temporarily store user information when they are verifyng
            //the second factor in the two-factor authentication process
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            //Enables the application to remember the second login verification factor such as phone or email
            //Once you check this option, your second step of verification during the login process will be
            //remembered on the device where you logged in from.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers  
            //app.UseMicrosoftAccountAuthentication(  
            //    clientId: "",  
            //    clientSecret: "");  

            //app.UseTwitterAuthentication(  
            //   consumerKey: "",  
            //   consumerSecret: "");  

            //app.UseFacebookAuthentication(  
            //   appId: "",  
            //   appSecret: "");  

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()  
            //{  
            //    ClientId = "",  
            //    ClientSecret = ""  
            //});  
        }
    }
}