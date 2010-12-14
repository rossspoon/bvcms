using System.Web.Mvc;

namespace CmsWeb.Areas.Setup
{
    public class SetupAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Setup";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Setup_default",
                "Setup/{controller}/{action}/{id}",
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
