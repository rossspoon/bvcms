using System.Web.Mvc;
using System.Web.Routing;

namespace CMSRegCustom
{
    public static class Routes
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapAreaRoute("CMSRegCustom", "RegCustom_Default",
                "{controller}/{action}/{id}",
                new { controller = "RegCustomHome", action = "Index", id = "" },
                new string[] { "CMSRegCustom.Controllers" }
            );
        }
    }
}
