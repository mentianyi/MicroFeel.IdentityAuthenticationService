using System;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Security;

namespace MicroFeel.AuthenticationService
{
    internal class AuthenticationServiceExtension
    {
        internal static void AuthenticationService_Authenticating<T>(object sender, AuthenticatingEventArgs e) where T : IDisposable, ClassLibrary1.IAuthenticationUserManager, new()
        {
            using (T userManager = new T())
            {
                e.Authenticated = userManager.Validuser(e.UserName, e.Password);
                e.AuthenticationIsComplete = true;
            }
        }

        internal static void AuthenticationService_CreatingCookie<T>(object sender, CreatingCookieEventArgs e) where T : IDisposable, ClassLibrary1.IAuthenticationUserManager, new()
        {
            var dt = DateTime.Now;

            using (T userManager = new T())
            {
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,
                    e.UserName,
                    dt,
                    dt.AddDays(1),
                    e.IsPersistent,
                    string.Join(",", userManager.getRoles(e.UserName)),
                    FormsAuthentication.FormsCookiePath);

                HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)) { Expires = ticket.Expiration });
                HttpContext.Current.Response.Cookies.Add(new HttpCookie("Roles", Convert.ToBase64String(System.Text.UTF8Encoding.Default.GetBytes(string.Join(",", userManager.getRoles(e.UserName))))));
                e.CookieIsSet = true;
            }
        }
    }
}