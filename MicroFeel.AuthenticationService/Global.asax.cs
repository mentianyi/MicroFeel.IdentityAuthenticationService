using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace MicroFeel.AuthenticationService
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //Custom AuthenticationService methods
            System.Web.ApplicationServices.AuthenticationService.Authenticating += AuthenticationServiceExtension.AuthenticationService_Authenticating;
            System.Web.ApplicationServices.AuthenticationService.CreatingCookie += AuthenticationServiceExtension.AuthenticationService_CreatingCookie;

            //Custom RoleService methods
            //System.Web.ApplicationServices.RoleService.SelectingProvider += RoleService_SelectingProvider;
        }

        //private void RoleService_SelectingProvider(object sender, System.Web.ApplicationServices.SelectingProviderEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

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