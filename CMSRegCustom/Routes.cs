using System.Web.Mvc;
using System.Web.Routing;

namespace CMSWebCommon
{
    public static class Routes
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapAreaRoute("CMSWebCommon", "Common_Default",
                "{controller}/{action}/{id}",
                new { controller = "CommonHome", action = "Index", id = "" },
                new string[] { "CMSWebCommon.Controllers" }
            );
        }
    }
}
