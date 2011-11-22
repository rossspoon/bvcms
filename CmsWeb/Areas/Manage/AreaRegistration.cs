using System.Web.Mvc;

namespace CmsWeb.Areas.Manage
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
            AddRoute(context, "Merge");
            AddRoute(context, "OrgMembers");
            AddRoute(context, "Batch");
            AddRoute(context, "Promotion");
            AddRoute(context, "Volunteers");
            context.MapRoute(
                "Public_Logon",
                "Logon",
                new { controller = "Account", action = "Logon", id="" }
            );
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
