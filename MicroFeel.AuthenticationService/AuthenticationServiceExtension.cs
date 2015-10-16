using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Security;

namespace MicroFeel.AuthenticationService
{
    public class AuthenticationServiceExtension
    {
        internal static void AuthenticationService_Authenticating(object sender, AuthenticatingEventArgs e)
        {
            using (var userManager = CustomUserManager.Create())
            {
                var userInfo = userManager.Find(e.UserName, e.Password);
                e.Authenticated = userInfo != null;
                e.AuthenticationIsComplete = true;
            }
        }

        internal static void AuthenticationService_CreatingCookie(object sender, System.Web.ApplicationServices.CreatingCookieEventArgs e)
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


            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.Expires = ticket.Expiration;

            HttpContext.Current.Response.Cookies.Add(cookie);
            e.CookieIsSet = true;
        }
    }
}