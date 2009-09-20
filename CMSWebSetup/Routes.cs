using System.Web.Mvc;
using System.Web.Routing;

namespace CMSWebSetup
{
    public static class Routes
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapAreaRoute("CMSWebSetup", "Display_Default",
            //    "CMSWebSetup/Display/{action}/{page}",
            //    new { controller = "Display", action = "Page", id = "" },
            //    new string[] { "CMSWebSetup.Controllers" }
            //    );
            routes.MapAreaRoute("CMSWebSetup", "Setup_Default",
                "{controller}/{action}/{id}",
                new { controller = "Home2", action = "Index", id = "" },
                new string[] { "CMSWebSetup.Controllers" }
            );
        }
    }
}
