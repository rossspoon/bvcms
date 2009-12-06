using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using CmsData;
using System.Net.Mail;
using System.Web.Configuration;

namespace Disciples
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                var u = DbUtil.Db.Users.Single(uu => uu.Username == User.Identity.Name);
                //if (u != null && (u.ForceLogin ?? false))
                //{
                //    u.ForceLogin = false;
                //    DbUtil.Db.SubmitChanges();
                //    FormsAuthentication.SignOut();
                //    Response.Redirect("~/");
                //}
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
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
            {
                var u = DbUtil.Db.Users.Single(uu => uu.Username == User.Identity.Name);
                if (u == null)
                {
                    FormsAuthentication.SignOut();
                    DbUtil.Db.CurrentUser = new User { Username = Request.AnonymousID };
                }
                else
                    DbUtil.Db.CurrentUser = u;
            }
            else
                DbUtil.Db.CurrentUser = new User { Username = Request.AnonymousID };
        }
        public void AnonymousIdentification_OnCreate(Object sender, AnonymousIdentificationEventArgs e)
        {
            e.AnonymousID = "anon_" + DateTime.Now.Ticks;
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            var debug = true;
#if DEBUG
            if (debug)
                return;
#endif
            var ex = Server.GetLastError();
            if (ex.Message == "File does not exist.")
                return;
            var u = DbUtil.Db.CurrentUser;
            var smtp = new SmtpClient();
            var msg = new MailMessage();
            msg.Subject = "disciples error on " + Request.Url.Authority;
            if (u != null && u.EmailAddress != null)
            {
                msg.From = new MailAddress(u.EmailAddress, u.Name);
                msg.Body = string.Format("\n{0} ({1}, {2})\n", u.EmailAddress, u.UserId, u.Name) + ex.ToString();
            }
            else
            {
                msg.From = new MailAddress(WebConfigurationManager.AppSettings["sysfromemail"]);
                msg.Body = ex.ToString();
            }
            msg.To.Add(WebConfigurationManager.AppSettings["senderrorsto"]);
            //smtp.Send(msg);
        }

        //public void FormsAuthentication_OnAuthenticate(object sender, FormsAuthenticationEventArgs args)
        //{
        //    if (FormsAuthentication.CookiesSupported)
        //    {
        //        if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
        //        {
        //            var c = Request.Cookies[FormsAuthentication.FormsCookieName];
        //        }
        //    }
        //    else
        //    {
        //        throw new HttpException("Cookieless Forms Authentication is not " +
        //                                "supported for this application.");
        //    }
        //}
    }
}