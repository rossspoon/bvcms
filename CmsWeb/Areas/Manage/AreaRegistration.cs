using System;
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
            AddRoute(context, "Duplicates");
            AddRoute(context, "UploadPeople");
            AddRoute(context, "OrgMembers");
            AddRoute(context, "Batch");
            AddRoute(context, "Promotion");
            AddRoute(context, "Error", "Account", "Error", "Error");
            context.MapRoute(
                "Public_Logon",
                "Logon",
                new { controller = "Account", action = "Logon", id="" }
            );
            context.MapRoute(
                "EmailsView",
                "Manage/Emails/View/{id}",
                new { controller = "EmailsView", action = "View", id = "" }
            );
			context.MapRoute("RequestVolunteers0",
				 "Manage/Volunteers/Request0/{ticks}/{oid}/{limit}",
				 new { controller = "Volunteers", action = "Request0", ticks = 0, oid = 0, limit = 0 }
			 );
			context.MapRoute("RequestVolunteers",
				 "Manage/Volunteers/Request/{mid}/{limit}",
				 new { controller = "Volunteers", action = "Request", mid = 0, limit = 0 }
			 );
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
        private static void AddRoute(AreaRegistrationContext context, string name, string controller, string path, string action)
        {
            context.MapRoute(name, path,
                new { controller = controller, action = action, id = "" });
        }
    }
}
