using System.Web.Mvc;

namespace CMSWeb.Areas.Public
{
    public class PublicAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Public";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            AddRoute(context, "DiscipleLife");
            AddRoute(context, "GODisciples");
            AddRoute(context, "LoveRespect");
            AddRoute(context, "MOBS");
            AddRoute(context, "Retreat");
            AddRoute(context, "OnlineReg");
            AddRoute(context, "Prayer");
            AddRoute(context, "RecReg");
            AddRoute(context, "Register");
            AddRoute(context, "Sales");
            AddRoute(context, "OptOut");
            AddRoute(context, "SoulMate");
            AddRoute(context, "StepClass");
            AddRoute(context, "VBSReg");
            AddRoute(context, "Event");
            AddRoute(context, "VolunteerConfirm", "Volunteer", "Volunteer/Confirm", "confirm");
            AddRoute(context, "VolunteerStart", "Volunteer", "Volunteer/{id}", "Start");
            AddRoute(context, "Volunteer");
            context.MapRoute(
                "Public_default",
                "Public/{controller}/{action}/{id}",
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
