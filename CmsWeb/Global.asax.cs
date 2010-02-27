using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CmsData;
using UtilityExtensions;
using CMSWeb;
using System.Net.Mail;
using System.Web.Configuration;
using CMSPresenter;
using System.Net;
using System.Text;
using System.IO;

namespace CMSWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            RouteTable.Routes.RouteExistingFiles = true;
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
            string smtppasswordfile = Server.MapPath("/Config/smtppassword.txt");
            if (File.Exists(smtppasswordfile))
            {
                var a = File.ReadAllText(smtppasswordfile).Split(',');
                Application["smtpcreds"] = a;
            }
#if DEBUG
            //HibernatingRhinos.Profiler.Appender.LinqToSql.LinqToSqlProfiler.Initialize();
#endif
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{myWebForms}.aspx/{*pathInfo}");
            routes.IgnoreRoute("{myWebForms}.ashx/{*pathInfo}");
            routes.IgnoreRoute("{myWebForms}.asmx/{*pathInfo}");
            routes.IgnoreRoute("{myWebForms}.js/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("{name}.png");
            routes.IgnoreRoute("Admin/{*pathInfo}");
            routes.IgnoreRoute("AppReview/{*pathInfo}");
            routes.IgnoreRoute("CustomErrors/{*pathInfo}");
            routes.IgnoreRoute("Contributions/{*pathInfo}");
            routes.IgnoreRoute("Report/{*pathInfo}");
            routes.IgnoreRoute("Dialog/{*pathInfo}");
            routes.IgnoreRoute("ckeditor/{*pathInfo}");
            routes.IgnoreRoute("StaffOnly/{*pathInfo}");
            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("App_Themes/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Errors/{*pathInfo}");
            routes.IgnoreRoute("demos/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");
            routes.IgnoreRoute("Upload/{*pathInfo}");
            routes.IgnoreRoute("{myWebPage}.htm");
            routes.IgnoreRoute("{myReport}.rdlc");
            routes.IgnoreRoute("{dir1}/{dir2}/{file}.js");
            routes.IgnoreRoute("{dir1}/{dir2}/{file}.css");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (Util.UserId == 0 && Util.UserName.HasValue())
            {
                var u = DbUtil.Db.Users.SingleOrDefault(us => us.Username == Util.UserName);
                if (u != null)
                {
                    Util.UserId = u.UserId;
                    Util.UserPeopleId = u.PeopleId;
                }
            }
            Util.SessionStarting = true;
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current != null)
                DbUtil.DbDispose();
            if (Response.Status.StartsWith("401")
                    && Request.Url.AbsolutePath.EndsWith(".aspx"))
                Login.CheckStaffRole(User.Identity.Name);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
#if DEBUG
            if (HttpContext.Current != null)
                return;
#endif
            var ex = Server.GetLastError();
            if (ex is HttpException)
            {
                var code = ((HttpException)ex).GetHttpCode();
                if (code == (int)HttpStatusCode.NotFound 
                    || code == (int)HttpStatusCode.Forbidden)
                    return;
            }
            
            var u = DbUtil.Db.CurrentUser;
            var smtp = Util.Smtp();
            var msg = new MailMessage();

            var sb = new StringBuilder();
            if (Request.RequestType == "POST")
                foreach (var s in Request.Form.AllKeys)
                    if (!s.Contains("Viewstate"))
                        sb.AppendFormat("\n{0}: {1}", s, Request.Form[s]);

            msg.Subject = "bvcms error on " + Request.Url.Authority;
            if (u != null)
            {
                msg.From = new MailAddress(u.EmailAddress, u.Name);
                msg.Body = "\n{0} ({1}, {2})\n{3}\n".Fmt(u.EmailAddress, u.UserId, u.Name, Request.Url.OriginalString) 
                    + ex.ToString() + sb.ToString();
            }
            else
            {
                msg.From = new MailAddress(WebConfigurationManager.AppSettings["errorsfromemail"]);
                msg.Body = "\n{0}\n".Fmt(Request.Url.OriginalString)
                    + ex.ToString() + sb.ToString();
            }
            foreach (var a in CMSRoleProvider.provider.GetRoleUsers("Developer"))
                msg.To.Add(new MailAddress(a.Person.EmailAddress, a.Name));
            smtp.Send(msg);
        }
   }
}