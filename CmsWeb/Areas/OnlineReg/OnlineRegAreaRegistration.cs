using System.Web.Mvc;
using UtilityExtensions;

namespace CmsWeb.Areas.OnlineReg
{
    public class OnlineRegAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "OnlineReg";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            AddRoute(context, "OnlineReg");
            context.MapRoute(
                "Public_CreateAccount",
                "CreateAccount",
                new { controller = "OnlineReg", action = "Index", id = Util.CreateAccountCode.ToString() }
            );
            context.MapRoute(
                "Public_MyData",
                "MyData",
                new { controller = "OnlineReg", action = "Index", id = Util.CreateAccountCode.ToString() }
            );
            context.MapRoute(
                "OnlineReg_default",
                "OnlineReg/{controller}/{action}/{id}",
                new { action = "Index", id = "" }
            );
        }
        private void AddRoute(AreaRegistrationContext context, string controller)
        {
            context.MapRoute(controller, controller + "/{action}/{id}",
                new { controller = controller, action = "Index", id = "" });
        }
        private static void AddRoute(AreaRegistrationContext context, string name, string controller, string path, string action)
        {
            context.MapRoute(name, path,
                new { controller = controller, action = action, id = "" });
        }
    }
}
