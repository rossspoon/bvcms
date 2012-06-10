using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using UtilityExtensions;
using CmsData;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.IO;

namespace CmsWeb
{
	[MyRequireHttps]
	public class CmsController : CmsControllerNoHttps
	{
	}

	public class CmsControllerNoHttps : Controller
    {
        protected override void HandleUnknownAction(string actionName)
        {
            //base.HandleUnknownAction(actionName);
            throw new HttpException(404, "404");
        }
        public static string HeaderHtml(string altcontent, string headertext, string logoimg)
        {
            var c = DbUtil.Content("Site2Header" + altcontent) ?? DbUtil.Content("Site2Header");
        	if (c != null)
                return c.Body;
            return @"
		<div id=""header"">
		   <div id=""title"">
		      <h1><img alt=""logo"" src='{0}' align=""middle"" />&nbsp;{1}</h1>
		   </div>
		</div>".Fmt(logoimg, headertext);
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            Util.Helpfile = "_{0}_{1}".Fmt(
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName);
        }
        public string AuthenticateDeveloper(bool log = false)
        {
            var auth = Request.Headers["Authorization"];
            if (auth.HasValue())
            {
                var cred = System.Text.ASCIIEncoding.ASCII.GetString(
                    Convert.FromBase64String(auth.Substring(6))).Split(':');
                var username = cred[0];
                var password = cred[1];

                string ret = null;
                var valid = CMSMembershipProvider.provider.ValidateUser(username, password);
                if (valid)
                {
                    var roles = CMSRoleProvider.provider;
                    var u = CmsWeb.Models.AccountModel.SetUserInfo(username, Session);
                    if (!roles.IsUserInRole(username, "Developer"))
                        valid = false;
                }
                if (valid)
                    ret = " API {0} authenticated".Fmt(username);
                else
                    ret = "!API {0} not authenticated".Fmt(username);
                if (log)
                    DbUtil.LogActivity(ret.Substring(1));
                return ret;
            }
            return "!API no Authorization Header";
        }
		public static string RenderPartialViewToString(Controller controller, string viewName, object model)
		{
			controller.ViewData.Model = model;
			try
			{
				using (var sw = new StringWriter())
				{
					ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
					ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
					viewResult.View.Render(viewContext, sw);
					return sw.GetStringBuilder().ToString();
				}
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
		}
    }
   [MyRequireHttps]
    public class CmsStaffController : Controller
    {
        public bool NoCheckRole { get; set; }

        protected override void HandleUnknownAction(string actionName)
        {
            //base.HandleUnknownAction(actionName);
            throw new HttpException(404, "404");
        }
        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            if (!User.Identity.IsAuthenticated)
            {
                var s = "/Logon?ReturnUrl=" + HttpUtility.UrlEncode(Request.RawUrl);
                if (Request.QueryString.Count > 0)
                    s += "&" + Request.QueryString.ToString();
                filterContext.Result = Redirect(s);
            }
            else if (!NoCheckRole)
            {
                var r = Models.AccountModel.CheckAccessRole(Util.UserName);
                if (r.HasValue())
                    filterContext.Result = Redirect(r);
            }
            base.OnActionExecuting(filterContext);
            Util.Helpfile = "_{0}_{1}".Fmt(
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName);
        }
        public ActionResult RedirectShowError(string message)
        {
            return new RedirectResult(
                "/Home/ShowError/?error={0}&url={1}".Fmt(Server.UrlEncode(message),
                Request.Url.OriginalString));
        }
    }
   [MyRequireHttps]
    public class CmsStaffAsyncController : System.Web.Mvc.AsyncController
    {
        public bool NoCheckRole { get; set; }

        protected override void HandleUnknownAction(string actionName)
        {
            //base.HandleUnknownAction(actionName);
            throw new HttpException(404, "404");
        }
        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            if (!User.Identity.IsAuthenticated)
            {
                var s = "/Logon?ReturnUrl=" + HttpUtility.UrlEncode(Request.RawUrl);
                if (Request.QueryString.Count > 0)
                    s += "&" + Request.QueryString.ToString();
                filterContext.Result = Redirect(s);
            }
            else if (!NoCheckRole)
            {
                var r = Models.AccountModel.CheckAccessRole(Util.UserName);
                if (r.HasValue())
                    filterContext.Result = Redirect(r);
            }
            base.OnActionExecuting(filterContext);
            Util.Helpfile = "_{0}_{1}".Fmt(
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName);
        }
    }
    public class RequireBasicAuthentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var req = filterContext.HttpContext.Request;
            if (!req.Headers["Authorization"].HasValue() && !req.Headers["username"].HasValue())
            {
                var res = filterContext.HttpContext.Response;
                res.StatusCode = 401;
                res.AddHeader("WWW-Authenticate", "Basic realm=\"{0}\"".Fmt(DbUtil.Db.Host));
                res.End();
            }
        }
    }

    public class SessionExpire : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Session != null)
                if (context.Session.IsNewSession)
                {
                    string sessionCookie = context.Request.Headers["Cookie"];
                    if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                        filterContext.Result = new RedirectResult("/Errors/SessionTimeout.htm");
                }
            base.OnActionExecuting(filterContext);
        }
    }
    public class DataGridResult : ActionResult
    {
        DataGrid dg;
		bool excel;
        public DataGridResult(IEnumerable list, bool excel = false)
        {
            dg = new DataGrid();
            dg.DataSource = list;
			this.excel = excel;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;

			if (excel)
			{
				Response.Buffer = true;
				Response.ContentType = "application/vnd.ms-excel";
				Response.AddHeader("Content-Disposition", "attachment;filename=CMSPeople.xls");
			}
        	Response.Charset = "";
            dg.DataBind();
            dg.RenderControl(new HtmlTextWriter(Response.Output));
        }
    }
    public class MyRequireHttpsAttribute : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext != null)
			{
	            if(filterContext.HttpContext.Request.IsLocal)
	                return;
				if (!WebConfigurationManager.AppSettings["cmshost"].StartsWith("https:"))
					return;
            }
            base.OnAuthorization(filterContext);
        }
    }
}
