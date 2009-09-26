using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CmsData;
using UtilityExtensions;
using System.Configuration;
using CMSWeb;
using System.Web.Security;
using System.Web.DynamicData;
using System.Data.Linq.Mapping;
using System.Collections;
using System.Web.SessionState;
using System.Xml.Linq;
using System.Net.Mail;
using System.Web.Configuration;
using CMSPresenter;

namespace CMSWeb2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(new AreaViewEngine());
            RegisterRoutes(RouteTable.Routes);
            //var model = new System.Web.DynamicData.MetaModel();
            //model.RegisterContext(typeof(CMSDataContext), new ContextConfiguration() { ScaffoldAllTables = false });
        }
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{myWebForms}.aspx/{*pathInfo}");
            routes.IgnoreRoute("{myWebForms}.ashx/{*pathInfo}");
            routes.IgnoreRoute("{myWebForms}.asmx/{*pathInfo}");
            routes.IgnoreRoute("{myWebForms}.js/{*pathInfo}");
            routes.IgnoreRoute("{name}.ico");
            routes.IgnoreRoute("{name}.png");
            routes.IgnoreRoute("Admin/{*pathInfo}");
            routes.IgnoreRoute("AppReview/{*pathInfo}");
            routes.IgnoreRoute("CustomErrors/{*pathInfo}");
            routes.IgnoreRoute("Contributions/{*pathInfo}");
            routes.IgnoreRoute("Report/{*pathInfo}");
            routes.IgnoreRoute("Dialog/{*pathInfo}");
            routes.IgnoreRoute("fckeditor/{*pathInfo}");
            routes.IgnoreRoute("StaffOnly/{*pathInfo}");
            routes.IgnoreRoute("images/{*pathInfo}");
            routes.IgnoreRoute("App_Themes/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");
            routes.IgnoreRoute("Upload/{*pathInfo}");
            routes.IgnoreRoute("{myWebPage}.htm");
            routes.IgnoreRoute("{myReport}.rdlc");
            routes.IgnoreRoute("ttt/{*pathInfo}");
            routes.IgnoreRoute("{dir1}/{dir2}/{file}.js");
            routes.IgnoreRoute("{dir1}/{dir2}/{file}.css");

            routes.RouteExistingFiles = true;

            CMSWebCommon.Routes.RegisterRoutes(routes);
            CMSRegCustom.Routes.RegisterRoutes(routes);
            CMSWebSetup.Routes.RegisterRoutes(routes);

            routes.MapAreaRoute("Task", "Task_Default", 
                "Task/{action}/{id}", 
                new { controller = "Task", action = "List", id = "" },
                new string[] {"CMSWeb.Controllers"}
                );
            routes.MapAreaRoute("QueryBuilder", "QueryBuilder_Default", 
                "QueryBuilder/{action}/{id}",
                new { controller = "QueryBuilder", action = "Main", id = "" },
                new string[] { "CMSWeb.Controllers" }
                );
            routes.MapAreaRoute("StepClass", "StepClass_Default", 
                "StepClass/{action}",
                new { controller = "StepClass", action = "Step1", id = "" },
                new string[] { "CMSWeb.Controllers" }
                );
            routes.MapAreaRoute("VolunteereConfirm", "VolunteereConfirm_Default", 
                "Volunteere/Confirm",
                new { controller = "Volunteer", action = "confirm", id = "" },
                new string[] { "CMSWeb.Controllers" }
                );
            routes.MapAreaRoute("Volunteer", "Volunteer_Default", 
                "Volunteer/{id}",
                new { controller = "Task", action = "Start", id = "" },
                new string[] { "CMSWeb.Controllers" }
                );
            routes.MapAreaRoute("TaskDetail", "TaskDetail_Default", 
                "Task/Detail/{id}/Row/{rowid}",
                new { controller = "Task", action = "Detail", id = "" },
                new string[] { "CMSWeb.Controllers" }
                );
            routes.MapAreaRoute("Main", "Main_Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" },
                new string[] { "CMSWeb.Controllers" });
            
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
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
        //protected void Application_BeginRequest(object sender, EventArgs e)
        //{
        //}
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
            var ex = Server.GetLastError();
            var InDebug = false;
#if DEBUG
            InDebug = false;
#endif
            if (InDebug)
            {
                Response.Write("<html><body><pre>\n");
                Response.Write(ex.ToString());
                Response.Write("</pre></body></html>\n");
                Server.ClearError();
                return;
            }
            var u = DbUtil.Db.CurrentUser;
            var smtp = new SmtpClient();
            var msg = new MailMessage();
            msg.Subject = "bvcms error on " + Request.Url.Authority;
            if (u != null)
            {
                msg.From = new MailAddress(u.EmailAddress, u.Name);
                msg.Body = "\n{0} ({1}, {2})\n".Fmt(u.EmailAddress, u.UserId, u.Name) + ex.ToString();
            }
            else
            {
                msg.From = new MailAddress(WebConfigurationManager.AppSettings["sysfromemail"]);
                msg.Body = ex.ToString();
            }
            foreach (var a in CMSRoleProvider.provider.GetRoleUsers("Developer"))
                msg.To.Add(new MailAddress(a.Person.EmailAddress, a.Name));
            smtp.Send(msg);
        }
    }
}