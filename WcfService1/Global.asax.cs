using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WcfService1
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;

            if (request.Cookies.Count > 0)
            {
                var tokenValue = request.Cookies[0].Name;
                var formsTicket = FormsAuthentication.Decrypt(tokenValue);
                var myIdentity = new GenericIdentity(formsTicket.Name);
                var principal = new GenericPrincipal(myIdentity, new string[] {formsTicket.UserData });
                Context.User = principal;
                System.Threading.Thread.CurrentPrincipal = principal;
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}