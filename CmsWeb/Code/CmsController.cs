using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilityExtensions;
using CmsData;
using System.Web.Mvc;
using CmsWeb.Areas.Manage.Controllers;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace CmsWeb
{
    public class CmsController : System.Web.Mvc.Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }
        protected override void HandleUnknownAction(string actionName)
        {
            //base.HandleUnknownAction(actionName);
            throw new HttpException(404, "404");
        }
        public static string HeaderHtml(string altcontent, string headertext, string logoimg)
        {
            var c = DbUtil.Content("Site2Header" + altcontent);
            if (c == null)
                c = DbUtil.Content("Site2Header");
            if (c != null)
                return c.Body;
            return @"
		<div id=""header"">
		   <div id=""title"">
		      <h1><img alt=""logo"" src='{0}' align=""middle"" />&nbsp;{1}</h1>
		   </div>
		</div>".Fmt(logoimg, headertext);
        }
        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            Util.Helpfile = "{0}_{1}".Fmt(
                filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                filterContext.ActionDescriptor.ActionName);
        }
        protected override void ExecuteCore()
        {
            base.ExecuteCore();
        }
    }
    public class CmsStaffController : System.Web.Mvc.Controller
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
                filterContext.Result = Redirect("/Logon?ReturnUrl=" + filterContext.HttpContext.Request.Path);
            else if (!NoCheckRole)
            {
                var r = AccountController.CheckAccessRole(Util.UserName);
                if (r.HasValue())
                    filterContext.Result = Redirect(r);
            }
            base.OnActionExecuting(filterContext);
            Util.Helpfile = "{0}_{1}".Fmt(
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
                filterContext.Result = Redirect("/Logon?ReturnUrl=" + filterContext.HttpContext.Request.Path);
            else if (!NoCheckRole)
            {
                var r = AccountController.CheckAccessRole(Util.UserName);
                if (r.HasValue())
                    filterContext.Result = Redirect(r);
            }
            base.OnActionExecuting(filterContext);
            Util.Helpfile = "{0}_{1}".Fmt(
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
        public DataGridResult(DataGrid dg)
        {
            this.dg = dg;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var Response = context.HttpContext.Response;
            dg.DataBind();
            dg.RenderControl(new HtmlTextWriter(Response.Output));
        }
    }
}
