using System.Web.Mvc;

namespace CMSWeb.Areas.Manage
{
    public class ManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Manage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            AddRoute(context, "Account");
            AddRoute(context, "Display");
            AddRoute(context, "Home");
            AddRoute(context, "OrgMembers");
            AddRoute(context, "Groups");
            AddRoute(context, "Promotion");
            AddRoute(context, "Recreation");
            AddRoute(context, "VBS");
            AddRoute(context, "Volunteers");
            context.MapRoute(
                "Manage_default",
                "Manage/{controller}/{action}/{id}",
                new { action = "Index", id = "" }
            );
        }
        private void AddRoute(AreaRegistrationContext context, string controller)
        {
            context.MapRoute(controller, controller + "/{action}/{id}",
                new { controller = controller, action = "Index", id = "" });
        }
    }
}
