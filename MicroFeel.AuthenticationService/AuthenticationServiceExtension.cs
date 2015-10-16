using Microsoft.AspNet.Identity;
using System;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Security;

namespace MicroFeel.AuthenticationService
{
    internal class AuthenticationServiceExtension
    {
        internal static void AuthenticationService_Authenticating(object sender, AuthenticatingEventArgs e)
        {
            using (var userManager = CustomUserManager.Create())
            {
                e.Authenticated = userManager.ValidUser(e.UserName, e.Password);
                e.AuthenticationIsComplete = true;
            }
        }

        internal static void AuthenticationService_CreatingCookie(object sender, CreatingCookieEventArgs e)
        {
            var dt = DateTime.Now;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    e.UserName,
                    dt,
                    dt.AddMinutes(30),
                    e.IsPersistent,
                    e.CustomCredential,
                    FormsAuthentication.FormsCookiePath);

            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)) { Expires = ticket.Expiration });
            e.CookieIsSet = true;
        }
    }
}