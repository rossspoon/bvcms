using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using DiscData;

namespace BellevueTeachers
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
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
                DbUtil.Db.CurrentUser = DbUtil.Db.GetUser(User.Identity.Name);
            else
                DbUtil.Db.CurrentUser = new User { Username = Request.AnonymousID };
        }
        public void AnonymousIdentification_OnCreate(Object sender, AnonymousIdentificationEventArgs e)
        {
            e.AnonymousID = "anon_" + DateTime.Now.Ticks;
        }
    }
}