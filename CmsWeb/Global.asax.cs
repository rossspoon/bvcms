using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CmsData;
using UtilityExtensions;
using CmsWeb;
using System.Net.Mail;
using System.Web.Configuration;
using CMSPresenter;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using System.Globalization;
using CmsWeb.Areas.Manage.Controllers;
using System.Web.Caching;
using Elmah;
using System.Web.Security;

namespace CmsWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ModelMetadataProviders.Current = new DataAnnotationsModelMetadataProvider();
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
            RouteTable.Routes.RouteExistingFiles = true; 
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
            routes.IgnoreRoute("*/pagerror.gif");
            routes.IgnoreRoute("*/refresh.gif");
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
            routes.IgnoreRoute("elmah.axd");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
                Models.AccountModel.SetUserInfo(Util.UserName, Session);
                //Models.AccountModel.SetUserInfo("trecord", Session);
            Util.SysFromEmail = WebConfigurationManager.AppSettings["sysfromemail"];
			Util.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Util.SessionStarting = true;
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
//			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
//			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB"); 
        }
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current != null)
                DbUtil.DbDispose();
            if (Response.Status.StartsWith("401")
                    && Request.Url.AbsolutePath.EndsWith(".aspx"))
            {
                var r = Models.AccountModel.CheckAccessRole(User.Identity.Name);
                if (r.HasValue())
                    Response.Redirect(r);
            }
        }
        public void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            var httpException = e.Exception as HttpException;
            if (httpException != null && httpException.GetHttpCode() == 404)
                e.Dismiss();
        }
    }
}