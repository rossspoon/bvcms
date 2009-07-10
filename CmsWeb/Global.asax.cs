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

namespace CMSWeb2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
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

            routes.MapRoute("Cache",
                "cache/{action}/{key}/{version}",
                new { controller = "Cache", action = "Content", key = "", version = "" });
            routes.MapRoute("Task",
                "Task/{action}/{id}",
                new { controller = "Task", action = "List", id = "" });
            routes.MapRoute("QB",
                "QueryBuilder/{action}/{id}",
                new { controller = "QueryBuilder", action = "Main", id = "" });
            routes.MapRoute("Display",
                "Display/{action}/{page}",
                new { controller = "Display", action = "Page", page = "" });
            routes.MapRoute("StepClass",
                "StepClass/{action}",
                new { controller = "StepClass", action = "Step1" });
            routes.MapRoute("VolunteerConfirm",
                "Volunteer/Confirm",
                new { controller = "Volunteer", action = "confirm", id = "" });
            routes.MapRoute("VolunteerHome",
                "Volunteer/{id}",
                new { controller = "Volunteer", action = "Start", id = "" });
            routes.MapRoute("TaskDetailRow",
                "Task/Detail/{id}/Row/{rowid}",
                new { controller = "Task", action = "Detail", id = "" });
            routes.MapRoute("Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" });
        }

        public static void StartSession()
        {
            if (Util.UserId == 0 && Util.UserName.HasValue())
            {
                var u = DbUtil.Db.Users.Single(us => us.Username == Util.UserName);
                Util.UserId = u.UserId;
                Util.UserPeopleId = u.PeopleId;
            }
            Util.SessionStarting = true;
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            StartSession();
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
            var err = Server.GetLastError();
            if ((HttpContext.Current != null) && User.IsInRole("Developer") && Util.UserName.ToLower() != "karenw")
            {
                Response.Clear();
                Response.Write("<h3>{0}</h3>\n".Fmt(err.Message));
                Response.Write("<div style='font-family:Courier New'>\n{0}\n</div>".Fmt(err.ToString().Replace("\n", "<br/>\n")));
                Server.ClearError();
            }
            else
            {
                Emailer em = null;
                var u = Membership.GetUser(User.Identity.Name);
                if (u != null && u.Email.HasValue())
                    em = new Emailer(u.Email, u.UserName);
                else
                    em = new Emailer();
                foreach (var name in Roles.GetUsersInRole("Developer"))
                {
                    var d = Membership.GetUser(name);
                    em.LoadAddress(d.Email, name);
                }
                em.NotifyEmail("CMS2 error for user: " + User.Identity.Name,
                    "<h3>{0}</h3>\n<div style='font-family:Courier New'>\n{1}\n</div>"
                    .Fmt(err.Message, err.ToString().Replace("\n", "<br/>\n")));
            }
        }
    }
}