using System.Web.Mvc;
using System.Web.Routing;

namespace CMSWebSetup
{
    public class Routes : AreaRegistration
    {
        public override string AreaName
        {
            get { return "CMSWebSetup"; }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            AddRoute(context, "Display");
            AddRoute(context, "Fund");
            AddRoute(context, "MetroZip");
            AddRoute(context, "Ministry");
            AddRoute(context, "Program");
            AddRoute(context, "MemberType");
            AddRoute(context, "Lookup");
            AddRoute(context, "PromotionSetup");
            AddRoute(context, "RecreationSetup");
            AddRoute(context, "Setting");
            AddRoute(context, "UsersCanEmailFor");
            AddRoute(context, "VolOpportunity");
        }
        private void AddRoute(AreaRegistrationContext context, string controller)
        {
            context.MapRoute(controller, controller + "/{action}/{id}",
                new { controller = controller, action = "Index", id = "" });
        }
    }
}
