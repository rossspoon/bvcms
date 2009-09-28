using System.Web.Mvc;
using System.Web.Routing;

namespace CMSWebSetup
{
    public static class Routes
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.AddRoute("Display");
            routes.AddRoute("Fund");
            routes.AddRoute("MetroZip");
            routes.AddRoute("Ministry");
            routes.AddRoute("Program");
            routes.AddRoute("PromotionSetup");
            routes.AddRoute("RecreationSetup");
            routes.AddRoute("Setting");
            routes.AddRoute("UsersCanEmailFor");
            routes.AddRoute("VolOpportunity");
        }
        private static void AddRoute(this RouteCollection routes, string controller)
        {
            routes.MapAreaRoute("CMSWebSetup", controller, controller + "/{action}/{id}",
                new { controller = controller, action = "Index", id = "" },
                new string[] { "CMSWebSetup.Controllers" });
        }
    }
}
