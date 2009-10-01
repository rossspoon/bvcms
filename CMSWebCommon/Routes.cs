using System.Web.Mvc;
using System.Web.Routing;

namespace CMSWebCommon
{
    public static class Routes
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.AddRoute("SearchPeople");
        }
        private static void AddRoute(this RouteCollection routes, string controller)
        {
            routes.MapAreaRoute("CMSWebCommon", controller, controller + "/{action}/{id}",
                new { controller = controller, action = "Index", id = "" },
                new string[] { "CMSWebCommon.Controllers" });
        }
    }
}
